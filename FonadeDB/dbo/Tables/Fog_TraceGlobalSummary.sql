CREATE TABLE [dbo].[Fog_TraceGlobalSummary] (
    [spid]            INT          NOT NULL,
    [DeadlockCount]   NUMERIC (18) NOT NULL,
    [RecompileCount]  NUMERIC (18) NOT NULL,
    [EscalationCount] NUMERIC (18) NOT NULL,
    [OtherCount]      NUMERIC (18) NOT NULL,
    [LastCollectTime] DATETIME     NULL,
    [SQLUser]         [sysname]    NULL,
    [HostName]        [sysname]    NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [SpotTraceEventSummary_Spid_Idx]
    ON [dbo].[Fog_TraceGlobalSummary]([spid] ASC) WITH (FILLFACTOR = 50);

