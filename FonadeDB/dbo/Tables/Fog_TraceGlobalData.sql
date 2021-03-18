CREATE TABLE [dbo].[Fog_TraceGlobalData] (
    [EventClass]      [sysname]        NULL,
    [IntegerData]     DECIMAL (18)     NULL,
    [TextData]        NVARCHAR (1000)  NULL,
    [Severity]        DECIMAL (18)     NULL,
    [TransactionID]   VARBINARY (20)   NULL,
    [ConnectionID]    DECIMAL (18)     NULL,
    [NTDomain]        NVARCHAR (30)    NULL,
    [Host]            NVARCHAR (50)    NULL,
    [SQLUser]         NVARCHAR (30)    NULL,
    [TracedSpid]      NUMERIC (20)     NULL,
    [Duration]        DECIMAL (18)     NULL,
    [StartTime]       DATETIME         NULL,
    [EndTime]         DATETIME         NULL,
    [Writes]          DECIMAL (18)     NULL,
    [CPUUsage]        DECIMAL (18)     NULL,
    [id]              INT              IDENTITY (1, 1) NOT NULL,
    [ClassName]       VARCHAR (50)     NULL,
    [ObjectID]        DECIMAL (18)     NULL,
    [ObjectName]      [sysname]        NULL,
    [dbid]            DECIMAL (18)     NULL,
    [IndexID]         DECIMAL (18)     NULL,
    [IndexName]       [sysname]        NULL,
    [CollectTime]     DATETIME         NULL,
    [SubClass]        DECIMAL (18)     NULL,
    [NTUser]          [sysname]        NULL,
    [HostProcessID]   DECIMAL (18)     NULL,
    [ApplicationName] VARCHAR (128)    NULL,
    [Resource]        VARCHAR (255)    NULL,
    [VarbinaryData]   VARBINARY (3000) NULL,
    [DatabaseName]    [sysname]        NULL,
    [ColumnName]      [sysname]        NULL,
    [RowAvailable]    CHAR (1)         NULL,
    [SQLStatement]    NTEXT            NULL,
    [Reads]           INT              NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [CL_UQ_id_Idx]
    ON [dbo].[Fog_TraceGlobalData]([id] ASC) WITH (FILLFACTOR = 50);


GO
CREATE NONCLUSTERED INDEX [EventClass_Idx]
    ON [dbo].[Fog_TraceGlobalData]([EventClass] ASC) WITH (FILLFACTOR = 50);


GO
CREATE NONCLUSTERED INDEX [TracedSpid_EventClass_StartTime]
    ON [dbo].[Fog_TraceGlobalData]([TracedSpid] ASC, [EventClass] ASC, [StartTime] ASC) WITH (FILLFACTOR = 50);

