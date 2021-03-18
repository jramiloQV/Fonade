CREATE TABLE [dbo].[Spot_Work_DBInfo] (
    [CollectTime]            DATETIME        NULL,
    [DBName]                 [sysname]       NULL,
    [DataSizeKB]             NUMERIC (18)    NULL,
    [DataUsedKB]             NUMERIC (18)    NULL,
    [LogSizeKB]              NUMERIC (18)    NULL,
    [LogUsedKB]              NUMERIC (18)    NULL,
    [FileCnt]                INT             NULL,
    [FileGroupCnt]           INT             NULL,
    [TableCnt]               INT             NULL,
    [TableKB]                NUMERIC (18)    NULL,
    [IndexCnt]               INT             NULL,
    [IndexKB]                NUMERIC (18)    NULL,
    [Disks]                  VARCHAR (500)   NULL,
    [DBStatus]               VARCHAR (20)    NULL,
    [LatestBackupTime]       DATETIME        NULL,
    [DBStatusInt]            INT             NULL,
    [DBOptions]              VARCHAR (255)   NULL,
    [DataPotentialGrowthKB]  NUMERIC (18)    NULL,
    [LogPotentialGrowthKB]   NUMERIC (18)    NULL,
    [DataCanGrow]            CHAR (1)        NULL,
    [DataPotentialGrowthPct] NUMERIC (16, 2) NULL,
    [LogCanGrow]             CHAR (1)        NULL,
    [LogPotentialGrowthPct]  NUMERIC (16, 2) NULL,
    [LogFlushes]             INT             NULL,
    [LogFlushesQuick]        INT             NULL,
    [LogFlushesFull]         INT             NULL,
    [CollectTimeFull]        DATETIME        NULL,
    [LogPctUsedIWatch]       DECIMAL (9, 3)  NULL,
    [crdate]                 DATETIME        NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [DBNameIdx]
    ON [dbo].[Spot_Work_DBInfo]([DBName] ASC) WITH (FILLFACTOR = 50);

