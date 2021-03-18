CREATE TABLE [dbo].[Fog_Trace_SQLBySPID] (
    [SPID]         INT      NULL,
    [StartTime]    DATETIME NULL,
    [CollectTime]  DATETIME NULL,
    [SQLStatement] NTEXT    NULL
);


GO
CREATE CLUSTERED INDEX [SQLBySPID_SPID]
    ON [dbo].[Fog_Trace_SQLBySPID]([SPID] ASC) WITH (FILLFACTOR = 50);

