CREATE TABLE [dbo].[Fog_TraceData] (
    [TracingSpid]     INT             CONSTRAINT [DF_Fog_TraceData_spid] DEFAULT (@@spid) NULL,
    [EventClass]      [sysname]       NULL,
    [IntegerData]     DECIMAL (18)    NULL,
    [TextData]        NVARCHAR (1000) NULL,
    [Severity]        DECIMAL (18)    NULL,
    [TransactionID]   VARBINARY (20)  NULL,
    [ConnectionID]    DECIMAL (18)    NULL,
    [NTDomain]        NVARCHAR (30)   NULL,
    [Host]            NVARCHAR (50)   NULL,
    [SQLUser]         NVARCHAR (30)   NULL,
    [TracedSpid]      INT             NULL,
    [Duration]        DECIMAL (18)    NULL,
    [StartTime]       DATETIME        NULL,
    [EndTime]         DATETIME        NULL,
    [Writes]          DECIMAL (18)    NULL,
    [CPUUsage]        DECIMAL (18)    NULL,
    [indent]          INT             NULL,
    [id]              INT             IDENTITY (1, 1) NOT NULL,
    [ClassName]       VARCHAR (50)    NULL,
    [ObjectID]        DECIMAL (18)    NULL,
    [ObjectName]      [sysname]       NULL,
    [dbid]            DECIMAL (18)    NULL,
    [IndexID]         DECIMAL (18)    NULL,
    [IndexName]       [sysname]       NULL,
    [Resource]        VARCHAR (255)   NULL,
    [SubClass]        NUMERIC (18)    NULL,
    [NTUser]          [sysname]       NULL,
    [HostProcessID]   NUMERIC (18)    NULL,
    [ApplicationName] VARCHAR (128)   NULL
);


GO
CREATE CLUSTERED INDEX [SPID_Idx]
    ON [dbo].[Fog_TraceData]([TracingSpid] ASC) WITH (FILLFACTOR = 50);

