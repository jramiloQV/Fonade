CREATE procedure dbo.Fog_MMPerfInfo(
	@Debug bit=0) as
begin
	set nocount on
	declare @PI	sysname
	set @PI = case when @@servername like '%\%' then 'MSSQL$' + @@servicename + ':Memory Manager' else 'SQLServer:Access Methods' end
	
	select
	sum (case when counter_name = 'Connection Memory (KB)'		then cntr_value else 0 end) as 'ConnectionMemoryKB',
	sum (case when counter_name = 'Granted Workspace Memory (KB)'	then cntr_value else 0 end) as 'GrantedWorkspaceMemoryKB',
	sum (case when counter_name = 'Lock Memory (KB)'		then cntr_value else 0 end) as 'LockMemoryKB',
	sum (case when counter_name = 'Lock Blocks Allocated'		then cntr_value else 0 end) as 'LockBlocksAllocated',
	sum (case when counter_name = 'Lock Owner Blocks Allocated'	then cntr_value else 0 end) as 'LockOwnerBlocksAllocated',
	sum (case when counter_name = 'Lock Blocks'			then cntr_value else 0 end) as 'LockBlocks',
	sum (case when counter_name = 'Lock Owner Blocks'		then cntr_value else 0 end) as 'LockOwnerBlocks',
	sum (case when counter_name = 'Maximum Workspace Memory (KB)'	then cntr_value else 0 end) as 'MaximumWorkspaceMemoryKB',
	sum (case when counter_name = 'Memory Grants Outstanding'	then cntr_value else 0 end) as 'MemoryGrantsOutstanding',
	sum (case when counter_name = 'Memory Grants Pending'		then cntr_value else 0 end) as 'MemoryGrantsPending',
	sum (case when counter_name = 'Optimizer Memory (KB)'		then cntr_value else 0 end) as 'OptimizerMemoryKB',
	sum (case when counter_name = 'SQL Cache Memory (KB)'		then cntr_value else 0 end) as 'SQLCacheMemoryKB',
	sum (case when counter_name = 'Target Server Memory(KB)'	then cntr_value else 0 end) as 'TargetServerMemoryKB',
	sum (case when counter_name = 'Total Server Memory (KB)'	then cntr_value else 0 end) as 'TotalServerMemoryKB'
	from master.dbo.sysperfinfo with (nolock) where object_name = @PI
end