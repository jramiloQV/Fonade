-- Debug: exec dbo.Fog_Jobs @LastCollected='2004-01-01 15:26:00'
-- select * from msdb.dbo.sysjobs
-- select * from msdb.dbo.sysjobservers
-- System usage: master.dbo.xp_sqlagent_enum_jobs, msdb.dbo.sysjobservers, msdb.dbo.syscategories
CREATE procedure dbo.Fog_Jobs(
	@LastCollected	datetime,
	@Debug bit=0)	as
begin
	set nocount on
	--
	-- See if the SQLAgent service is running.
	--
	declare @Name	sysname
	declare @Key	varchar(100)
	declare @Val	varchar(100)
	if @@servername like '%\%'
		set @Name = 'SQLAgent$' + @@servicename
	else
		set @Name = 'SQLServerAgent'
	set @Key = 'SYSTEM\CurrentControlSet\Services\' + @Name + '\'
	set @Val = null
	execute master.dbo.xp_regread 'HKEY_LOCAL_MACHINE', @Key, 'DisplayName',
					@Val output, 'no_output'
	declare @AgentRunning	bit
	create table #service_status(Status varchar(20))
	if @Val is not null
	begin
		insert into #service_status (Status)
			execute master.dbo.xp_servicecontrol 'querystate', @Name
	end
	if exists(select 1 from #service_status where Status = 'Running.')
		set @AgentRunning = 1
	else
		set @AgentRunning = 0
	drop table #service_status
	create table #enum_jobs(
		job_id			uniqueidentifier null,
		LastRunDate		integer		null,
		LastRunTime		integer		null,
		NextRunDate		integer		null,
		NextRunTime		integer		null,
		NextRunSchedID		integer		null,
		RequestedToRun		integer		null,
		RequestSource		integer		null,
		RequestSourceID		sysname		null,
		Running			integer		null,
		CurrentStep		integer		null,
		CurrentRetryAttempt	integer		null,
		State			integer		null,
	)
	if @AgentRunning = 1
	begin
		set nocount on
		insert into #enum_jobs (job_id, LastRunDate, LastRunTime, NextRunDate, NextRunTime, NextRunSchedID, RequestedToRun, RequestSource, RequestSourceID, Running, CurrentStep, CurrentRetryAttempt, State)
			execute master.dbo.xp_sqlagent_enum_jobs 1, ''
	end
	-- else #enum_jobs will be empty and nulls will exist in the join below
	
	set nocount off
	select
		j.name					as 'JobName', 
		isnull(c.name, 'Unknown')		as 'Category',
		j.enabled				as 'Enabled',
		case
			when @AgentRunning = 0 then 'Inactive'
			else 
				case x.State
					when 1 then 'Running'
					when 2 then 'Retry'
					when 3 then 'Retry'
					when 4 then 'Inactive'
					else 'Unknown'
				end 
		end					as 'CurrentStatus', 
		isnull(x.CurrentStep, 0)		as 'CurrentStepNbr',
		case when isdate(x.LastRunDate) = 1 then 
			stuff(stuff(right('00000000' + convert(varchar(10), x.LastRunDate), 8),
				7, 0, '-'), 5, 0, '-') + ' ' +
			stuff(stuff(right('00000000' + convert(varchar(10), x.LastRunTime), 6),
				5, 0, ':'), 3, 0, ':')
		else '' end				as 'LastRunTime', 
		case when isdate(x.NextRunDate) = 1 then 
			stuff(stuff(right('00000000' + convert(varchar(10), x.NextRunDate), 8),
				7, 0, '-'), 5, 0, '-') + ' ' +
			stuff(stuff(right('00000000' + convert(varchar(10), x.NextRunTime), 6),
				5, 0, ':'), 3, 0, ':')
		else '' end				as 'NextRunTime', 
		isnull(case s.last_run_outcome
			when 0 then 'Fail'
			when 1 then 'Success'
			when 2 then 'Retry'
			when 3 then 'Canceled'
			when 4 then 'Running'
			when 5 then 'Never Run'
			else 'Unknown'
			end, '')			as 'LastRunOutcome',
		isnull(s.last_run_duration, 0)		as 'LastRunDuration',
		case when
			s.last_run_outcome = 0
			and
			case when isdate(x.LastRunDate) = 1 then 
				stuff(stuff(right('00000000' + convert(varchar(10), x.LastRunDate), 8),
					7, 0, '-'), 5, 0, '-') + ' ' +
				stuff(stuff(right('00000000' + convert(varchar(10), x.LastRunTime), 6),
					5, 0, ':'), 3, 0, ':')
				else ''
			end > @LastCollected
		then 'yes'
		else 'no' end				as 'Alert'
	from
		msdb.dbo.sysjobs			j
		left join #enum_jobs			x on x.job_id = j.job_id
		left join msdb.dbo.sysjobservers	s on s.job_id = j.job_id
		left join msdb.dbo.syscategories	c on j.category_id = c.category_id
	order by 1
	drop table #enum_jobs
end