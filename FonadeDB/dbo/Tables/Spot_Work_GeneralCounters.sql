CREATE TABLE [dbo].[Spot_Work_GeneralCounters] (
    [GeneralCountersKey]             INT          NOT NULL,
    [SQLDTCStatus]                   INT          NULL,
    [SQLMSSearchStatus]              INT          NULL,
    [SQLServerMailStatus]            INT          NULL,
    [SQLAgentMailStatus]             INT          NULL,
    [SQLOLAPStatus]                  INT          NULL,
    [SQLServerAgentStatus]           INT          NULL,
    [DummaySQLServerAgentActiveJobs] INT          NULL,
    [LogicalDiskCountersDisabled]    INT          NULL,
    [ForegroundAppPriorityBoost]     INT          NULL,
    [ForegroundBoostText]            VARCHAR (30) NULL,
    [NTProcessID]                    INT          NULL,
    [ClusteredServer]                CHAR (1)     NULL,
    [ClusterNodeName]                VARCHAR (50) NULL,
    [SQLServerStatus]                INT          NULL,
    CONSTRAINT [PK_Spot_Work_GeneralCounters] PRIMARY KEY CLUSTERED ([GeneralCountersKey] ASC) WITH (FILLFACTOR = 50)
);

