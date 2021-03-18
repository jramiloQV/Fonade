CREATE TABLE [dbo].[Fog_Traces] (
    [TraceQueueNbr]            INT            NULL,
    [TracingSpid]              INT            CONSTRAINT [DF_Fog_Traces_TracingSpid] DEFAULT (@@spid) NULL,
    [TracingHostName]          VARCHAR (50)   NULL,
    [TracingProgramName]       VARCHAR (50)   NULL,
    [TracedSpid]               INT            NULL,
    [LastIndentValue]          SMALLINT       NULL,
    [LastCollectTime]          DATETIME       CONSTRAINT [DF_Fog_Traces_LastCollectTime] DEFAULT (getdate()) NULL,
    [DataCollectionInProgress] CHAR (1)       NULL,
    [LastCollectStartTime]     DATETIME       CONSTRAINT [DF_Fog_Traces_LastCollectStartTime] DEFAULT (getdate()) NULL,
    [LastCollectSpid]          INT            NULL,
    [TraceStartTime]           DATETIME       NULL,
    [TraceStartSpid]           INT            NULL,
    [DataLastFoundTime]        DATETIME       NULL,
    [LastQueueCheckTime]       DATETIME       NULL,
    [GetGlobalTraceSQL]        CHAR (1)       NULL,
    [LastGlobalTraceSQLSpid]   INT            NULL,
    [LastGlobalTraceSQLTime]   DATETIME       NULL,
    [TraceFileName]            NVARCHAR (500) NULL,
    [TraceFileNumber]          NUMERIC (18)   NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_Fog_Traces_QueueNbr]
    ON [dbo].[Fog_Traces]([TraceQueueNbr] ASC) WITH (FILLFACTOR = 50);


GO
CREATE UNIQUE CLUSTERED INDEX [IX_Fog_Traces_TracingSpid]
    ON [dbo].[Fog_Traces]([TracingSpid] ASC) WITH (FILLFACTOR = 50);

