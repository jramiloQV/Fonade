CREATE PROCEDURE [dbo].[Spot_CheckIfReplicationInstalled]
@ReplicationIsInstalled CHAR (1) OUTPUT, @ThisServerIsADistributor CHAR (1) OUTPUT, @ThisServerIsAPublisher CHAR (1) OUTPUT, @ThisServerIsASubscriber CHAR (1) OUTPUT, @DistributionServer [sysname] OUTPUT, @Print CHAR (1), @Debug CHAR (1), @ForceRefresh CHAR (1)
WITH ENCRYPTION
AS
BEGIN
--The script body was encrypted and cannot be reproduced here.
    RETURN
END


