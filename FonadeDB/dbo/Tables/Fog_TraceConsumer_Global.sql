CREATE TABLE [dbo].[Fog_TraceConsumer_Global] (
    [EventClass]      DECIMAL (18)   NULL,
    [EventSubClass]   DECIMAL (18)   NULL,
    [IntegerData]     DECIMAL (18)   NULL,
    [ServerName]      NVARCHAR (128) NULL,
    [BinaryData]      IMAGE          NULL,
    [TextData]        TEXT           NULL,
    [Severity]        DECIMAL (18)   NULL,
    [DatabaseID]      DECIMAL (18)   NULL,
    [ObjectID]        DECIMAL (18)   NULL,
    [IndexID]         DECIMAL (18)   NULL,
    [TransactionID]   BINARY (8)     NULL,
    [ConnectionID]    DECIMAL (18)   NULL,
    [NTUserName]      NVARCHAR (256) NULL,
    [NTDomainName]    NVARCHAR (256) NULL,
    [HostName]        NVARCHAR (128) NULL,
    [HostProcessID]   DECIMAL (18)   NULL,
    [ApplicationName] NVARCHAR (128) NULL,
    [SQLUserName]     NVARCHAR (128) NULL,
    [SPID]            DECIMAL (18)   NULL,
    [Duration]        DECIMAL (18)   NULL,
    [StartTime]       DATETIME       NULL,
    [EndTime]         DATETIME       NULL,
    [Reads]           DECIMAL (18)   NULL,
    [Writes]          DECIMAL (18)   NULL,
    [CPU]             DECIMAL (18)   NULL,
    [id]              INT            IDENTITY (1, 1) NOT NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ConsumerGlobal_id]
    ON [dbo].[Fog_TraceConsumer_Global]([id] ASC) WITH (FILLFACTOR = 50);


GO
CREATE CLUSTERED INDEX [ConsumerGlobal_SPIDEvntCls]
    ON [dbo].[Fog_TraceConsumer_Global]([SPID] ASC, [EventClass] ASC) WITH (FILLFACTOR = 50);

