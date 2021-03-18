CREATE TABLE [dbo].[Fog_TraceEventNames] (
    [EventClass] INT       NULL,
    [EventName]  [sysname] NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [PK_EventClass]
    ON [dbo].[Fog_TraceEventNames]([EventClass] ASC) WITH (FILLFACTOR = 50);

