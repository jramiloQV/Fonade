CREATE PROCEDURE [dbo].[Spot_CheckRefresh]
@CounterName VARCHAR (20), @ForceRefresh CHAR (1), @TimeForRefresh CHAR (1) OUTPUT, @ExtraDataDecimal DECIMAL (13) OUTPUT, @ExtraDataDecimal2 DECIMAL (13) OUTPUT, @ExtraDataTime DATETIME OUTPUT, @Print CHAR (1), @Debug CHAR (1), @Priority VARCHAR (20), @DBName [sysname], @Indent SMALLINT, @MinSecsBetweenRefresh INT OUTPUT, @LastRefreshTime DATETIME OUTPUT
WITH ENCRYPTION
AS
BEGIN
--The script body was encrypted and cannot be reproduced here.
    RETURN
END


