CREATE PROCEDURE [dbo].[Spot_VersionInfo]
@Debug CHAR (1), @Print CHAR (1), @PhysicalMemoryMB NUMERIC (18) OUTPUT, @SQLVersion VARCHAR (100) OUTPUT, @SQLEdition VARCHAR (100) OUTPUT, @ProcessorType VARCHAR (50) OUTPUT, @NTVersion VARCHAR (50) OUTPUT, @SPVersion VARCHAR (50) OUTPUT, @VersionID VARCHAR (20) OUTPUT, @ServerName VARCHAR (50) OUTPUT, @OutputFormat VARCHAR (10), @RunningOnACluster CHAR (1) OUTPUT, @ClusterMachineName [sysname] OUTPUT, @CaseSensitive INT OUTPUT
WITH ENCRYPTION
AS
BEGIN
--The script body was encrypted and cannot be reproduced here.
    RETURN
END


