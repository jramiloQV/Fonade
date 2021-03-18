CREATE procedure dbo.Fog_BMPerfInfo(
	@Debug bit=0) as
begin
	set nocount on
	declare @PI	sysname
	set @PI = case when @@servername like '%\%' then 'MSSQL$' + @@servicename + ':Buffer Manager' else 'SQLServer:Buffer Manager' end
	
-- The v7 flavored cname comes first, then the v2000 flavoured cname ...
	select
	sum (case when counter_name = 'Buffer Cache Hit Ratio'
		or counter_name = 'Buffer cache hit ratio'	then cntr_value else 0 end) as 'BufferCacheHitRatio',
	sum (case when counter_name = 'Buffer Cache Hit Ratio Base'
		or counter_name = 'Buffer cache hit ratio base'	then cntr_value else 0 end) as 'BufferCacheHitRatioBase',
	sum (case when counter_name = 'Page Requests/sec'
		or counter_name = 'Page lookups/sec'		then cntr_value else 0 end) as 'PageRequestsPS',
	sum (case when counter_name = 'Reserved Page Count'
		or counter_name = 'Reserved pages'		then cntr_value else 0 end) as 'ReservedPages',
	sum (case when counter_name = 'Stolen Page Count'
		or counter_name = 'Stolen pages'		then cntr_value else 0 end) as 'StolenPages',
	sum (case when counter_name = 'Lazy Writes/sec'
		or counter_name = 'Lazy writes/sec'		then cntr_value else 0 end) as 'LazyWritesPS',
	sum (case when counter_name = 'Readahead Pages/sec'
		or counter_name = 'Readahead pages/sec'		then cntr_value else 0 end) as 'ReadAheadPagesPS',
	sum (case when counter_name = 'Cache Size (pages)'
		or counter_name = 'Procedure cache pages'	then cntr_value else 0 end) as 'ProcedureCachePages',
	sum (case when counter_name = 'Page Reads/sec'
		or counter_name = 'Page reads/sec'		then cntr_value else 0 end) as 'PageReadsPS',
	sum (case when counter_name = 'Page Writes/sec'
		or counter_name = 'Page writes/sec'		then cntr_value else 0 end) as 'PageWritesPS',
	sum (case when counter_name = 'Checkpoint Writes/sec'
		or counter_name = 'Checkpoint pages/sec'	then cntr_value else 0 end) as 'CheckpointPagesPS'
	from master.dbo.sysperfinfo where object_name = @PI
-- Some cnames only exist in one version ...
-- only in v2000	'Free list stalls/sec'
-- only in v2000	'Free pages'
-- only in v2000	'Total pages'
-- only in v2000	'Target pages'
-- only in v2000	'Database pages'
-- only in v7		'ExtendedMem Requests/sec'
-- only in v7		'ExtendedMem Cache Hit Ratio'
-- only in v7		'ExtendedMem Cache Hit Ratio Base'
-- only in v7		'ExtendedMem Cache Migrations/sec'
-- only in v7		'Free Buffers'
-- only in v7		'Committed Pages'
-- only in v2000	'AWE lookup maps/sec'
-- only in v2000	'AWE stolen maps/sec'
-- only in v2000	'AWE write maps/sec'
-- only in v2000	'AWE unmap calls/sec'
-- only in v2000	'AWE unmap pages/sec'
-- only in v2000	'Page life expectancy'
-- Calculate sync writes as: page writes - checkpoint writes - lazywriter writes
-- set @SyncWrites = @SyncWrites - @CkptWrites - @LazyWrites
end