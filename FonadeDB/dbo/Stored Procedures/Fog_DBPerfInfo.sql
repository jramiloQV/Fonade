--
-- exec dbo.Fog_DBPerfInfo @Debug=1
-- select * from master.dbo.sysperfinfo where object_name = 'SQLServer:Databases' and instance_name = 'master'
--
CREATE procedure dbo.Fog_DBPerfInfo(
	@Debug bit=0) as
begin
	set nocount on
	declare @PI	sysname
	set @PI = case when @@servername like '%\%' then 'MSSQL$' + @@servicename + ':Databases' else 'SQLServer:Databases' end
	
	select
	convert(varchar, rtrim(instance_name))			as 'DBName',
	sum (case when counter_name = 'Data File(s) Size (KB)' then cntr_value else 0 end)	as 'DataFilesSizeKB',
	sum (case when counter_name = 'Log File(s) Size (KB)' then cntr_value else 0 end)	as 'LogFilesSizeKB',
--	sum (case when counter_name = 'Log File(s) Used Size (KB)' then cntr_value else 0 end)	as 'LogFilesUsedSizeKB',	-- not available in v7
	sum (case when counter_name = 'Percent Log Used' then cntr_value else 0 end)		as 'PercentLogUsed',
	sum (case when counter_name = 'Active Transactions' then cntr_value else 0 end)		as 'ActiveTransactions',
 	sum (case when counter_name = 'Transactions/sec' then cntr_value else 0 end)		as 'TransactionsPS',
-- 	sum (case when counter_name = 'Repl. Pending Xacts' then cntr_value else 0 end)		as 'ReplPendingXacts',
-- 	sum (case when counter_name = 'Repl. Trans. Rate' then cntr_value else 0 end)		as 'ReplTransRate',
 	sum (case when counter_name = 'Log Cache Reads/sec' then cntr_value else 0 end)		as 'LogCacheReadsPS',
-- 	sum (case when counter_name = 'Log Cache Hit Ratio' then cntr_value else 0 end)		as 'LogCacheHitRatio',		-- 'LogCachePhysicalReads'
-- 	sum (case when counter_name = 'Log Cache Hit Ratio Base' then cntr_value else 0 end)	as 'LogCacheHitRatioBase',	-- 'LogCacheLogicalReads',
 	sum (case when counter_name = 'Bulk Copy Rows/sec' then cntr_value else 0 end)		as 'BulkCopyRowsPS',
 	sum (case when counter_name = 'Bulk Copy Throughput/sec' then cntr_value else 0 end)	as 'BulkCopyThroughputPS',
 	sum (case when counter_name = 'Backup/Restore Throughput/sec' then cntr_value else 0 end)	as 'BackupThroughputPS',
 	sum (case when counter_name = 'DBCC Logical Scan Bytes/sec' then cntr_value else 0 end)	as 'DBCCLogicalScanBytesPS',	-- 'DBCCScans'	
 	sum (case when counter_name = 'Shrink Data Movement Bytes/sec' then cntr_value else 0 end)	as 'ShrinkDataMovementBytesPS',
 	sum (case when counter_name = 'Log Flushes/sec' then cntr_value else 0 end)			as 'LogFlushesPS',
-- 	-- Calculate LogBytesFlushedPS ...
-- 	if substring(@@version, 30, 1) = 7
-- 		set @n = (select cntr_value from #pi where counter_name = 'Log Bytes Per Flush')
-- 			* (select cntr_value from #pi where counter_name = 'Log Flushes/sec')
-- 	else
-- 		set @n = (select cntr_value from #pi where counter_name = 'Log Bytes Flushed/sec')
-- 
--	@n									as 'LogBytesFlushedPS',
-- 	sum (case when counter_name = 'Log Flush Waits/sec' then cntr_value else 0 end)		as 'LogFlushWaitsPS',
-- 	sum (case when counter_name = 'Log Flush Wait Time' then cntr_value else 0 end)		as 'LogFlushWaitTime',
	sum (case when counter_name = 'Log Truncations' then cntr_value else 0 end)		as 'LogTruncations',
	sum (case when counter_name = 'Log Growths' then cntr_value else 0 end)			as 'LogGrowths',
	sum (case when counter_name = 'Log Shrinks' then cntr_value else 0 end)			as 'LogShrinks'
	from master.dbo.sysperfinfo where object_name = @PI
	group by instance_name
end