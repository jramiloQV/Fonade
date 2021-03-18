CREATE procedure dbo.Fog_AMPerfInfo(
	@Debug bit=0) as
begin
	set nocount on
	declare @PI	sysname
	set @PI = case when @@servername like '%\%' then 'MSSQL$' + @@servicename + ':Access Methods' else 'SQLServer:Access Methods' end
	
	select
	sum (case when counter_name = 'Full Scans/sec'			then cntr_value else 0 end) as 'FullScansPS',
	sum (case when counter_name = 'Range Scans/sec'			then cntr_value else 0 end) as 'RangeScansPS',
	sum (case when counter_name = 'Probe Scans/sec'			then cntr_value else 0 end) as 'ProbeScansPS',
	sum (case when counter_name = 'Scan Point Revalidations/sec'	then cntr_value else 0 end) as 'ScanPointRevalidationsPS',
	sum (case when counter_name = 'Workfiles Created/sec'		then cntr_value else 0 end) as 'WorkfilesCreatedPS',
	sum (case when counter_name = 'Worktables Created/sec'		then cntr_value else 0 end) as 'WorktablesCreatedPS',
	sum (case when counter_name = 'Worktables From Cache Ratio'	then cntr_value else 0 end) as 'WorktablesFromCacheRatio',
	sum (case when counter_name = 'Worktables From Cache Base'	then cntr_value else 0 end) as 'WorktablesFromCacheBase',
	sum (case when counter_name = 'Forwarded Records/sec'		then cntr_value else 0 end) as 'ForwardedRecordsPS',
	sum (case when counter_name = 'Skipped Ghosted Records/sec'	then cntr_value else 0 end) as 'SkippedGhostedRecordsPS',
	sum (case when counter_name = 'Index Searches/sec'		then cntr_value else 0 end) as 'IndexSearchesPS',
	sum (case when counter_name = 'FreeSpace Scans/sec'		then cntr_value else 0 end) as 'FreeSpaceScansPS',
	sum (case when counter_name = 'FreeSpace Page Fetches/sec'	then cntr_value else 0 end) as 'FreeSpacePageFetchesPS',
	sum (case when counter_name = 'Pages Allocated/sec'		then cntr_value else 0 end) as 'PagesAllocatedPS',
	sum (case when counter_name = 'Extents Allocated/sec'		then cntr_value else 0 end) as 'ExtentsAllocatedPS',
	sum (case when counter_name = 'Mixed page allocations/sec'	then cntr_value else 0 end) as 'MixedPageAllocationsPS',
	sum (case when counter_name = 'Extent Deallocations/sec'	then cntr_value else 0 end) as 'ExtentDeallocationsPS',
	sum (case when counter_name = 'Page Deallocations/sec'		then cntr_value else 0 end) as 'PageDeallocationsPS',
	sum (case when counter_name = 'Page Splits/sec'			then cntr_value else 0 end) as 'PageSplitsPS',
	sum (case when counter_name = 'Table Lock Escalations/sec'	then cntr_value else 0 end) as 'TableLockEscalationsPS'
	from master.dbo.sysperfinfo with (nolock) where object_name = @PI
end