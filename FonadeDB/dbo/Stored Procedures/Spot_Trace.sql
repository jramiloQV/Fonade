CREATE PROCEDURE [dbo].[Spot_Trace]
@GlobalTrace CHAR (1), @EndTrace CHAR (1), @spid INT, @LastCallID INT, @RetainMins INT, @CheckMins INT, @Debug CHAR (1), @indent SMALLINT, @IncludeRecompiles CHAR (1), @IncludeEscalations CHAR (1), @IncludeTimeouts CHAR (1), @IncludeDeadlocks CHAR (1), @IncludeAutoStats CHAR (1), @MaxRowsPerRun INT, @MaxIDReturned INT OUTPUT, @NbrEventsPerLoop INT, @MinSecsBetweenDataCollection INT, @GetGlobalTraceSQL CHAR (1), @SkipCheck CHAR (1), @ShowSpotlightObjects CHAR (1), @ShowSystemObjects CHAR (1), @DummyCall CHAR (1), @xp_cmdshell_available CHAR (1)
WITH ENCRYPTION
AS
BEGIN
--The script body was encrypted and cannot be reproduced here.
    RETURN
END


