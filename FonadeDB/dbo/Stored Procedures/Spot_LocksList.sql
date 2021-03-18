CREATE PROCEDURE [dbo].[Spot_LocksList]
@spid INT, @MaxLocksForFullResourceInfo INT, @Print CHAR (1), @Debug CHAR (1), @LockDBName [sysname], @LockObjectName [sysname], @ShowDBShareLocks CHAR (1), @ShowIntentLocks CHAR (1), @ShowTempDBLocks CHAR (1), @ShowSystemObjects CHAR (1), @ShowSpotlightObjects CHAR (1), @FilterDBName [sysname], @FilterObjectName [sysname], @FilterSQLUser [sysname], @FilterNTUser [sysname], @FilterProgramName [sysname], @MaxLocks INT, @OutputFormat VARCHAR (20)
WITH ENCRYPTION
AS
BEGIN
--The script body was encrypted and cannot be reproduced here.
    RETURN
END


