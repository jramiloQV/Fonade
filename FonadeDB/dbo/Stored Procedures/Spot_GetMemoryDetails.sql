CREATE PROCEDURE [dbo].[Spot_GetMemoryDetails]
@SQLBufferCacheActiveKB NUMERIC (18) OUTPUT, @SQLBufferCacheFreeKB NUMERIC (18) OUTPUT, @SQLBufferCacheKB NUMERIC (18) OUTPUT, @SQLProcCacheActiveKB NUMERIC (18) OUTPUT, @SQLProcCacheKB NUMERIC (18) OUTPUT, @SQLCurrentMemoryKB NUMERIC (18) OUTPUT, @SQLMemoryConnectionsKB NUMERIC (18) OUTPUT, @SQLMemoryLocksKB NUMERIC (18) OUTPUT, @SQLMemoryOptimizerKB NUMERIC (18) OUTPUT, @SQLMemoryMinKB NUMERIC (18) OUTPUT, @SQLMemoryMaxKB NUMERIC (18) OUTPUT, @SQLMemorySortHashKB NUMERIC (18) OUTPUT, @SQLFreeListKB NUMERIC (18) OUTPUT, @SQLLogicalReads NUMERIC (18) OUTPUT, @SQLLogicalReadsInCache NUMERIC (18) OUTPUT, @SQLProcCacheLogicalReads NUMERIC (18) OUTPUT, @SQLProcCacheLogicalReadsInCache NUMERIC (18) OUTPUT, @SQLExtendedMemoryKB NUMERIC (18) OUTPUT, @PhysicalRAMKB NUMERIC (18) OUTPUT, @Print CHAR (1), @Debug CHAR (1), @ForceRefresh CHAR (1), @GetPhysicalRAMOnly CHAR (1)
WITH ENCRYPTION
AS
BEGIN
--The script body was encrypted and cannot be reproduced here.
    RETURN
END


