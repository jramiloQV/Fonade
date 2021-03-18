CREATE TABLE [dbo].[Spot_Work_FGInfo] (
    [CollectTime]                DATETIME        NULL,
    [DBName]                     [sysname]       NULL,
    [FGName]                     [sysname]       NULL,
    [FileCnt]                    INT             NULL,
    [SizeKB]                     NUMERIC (18)    NULL,
    [UsedKB]                     NUMERIC (18)    NULL,
    [Disks]                      VARCHAR (500)   NULL,
    [PotentialGrowthKB]          NUMERIC (18)    NULL,
    [CanGrow]                    CHAR (1)        NULL,
    [PotentialGrowthPct]         NUMERIC (16, 2) NULL,
    [TableKB]                    NUMERIC (18)    NULL,
    [IndexKB]                    NUMERIC (18)    NULL,
    [LogUncomittedKB]            NUMERIC (18)    NULL,
    [LogUndistributedKB]         NUMERIC (18)    NULL,
    [LogOldestTranVLF]           NUMERIC (18)    NULL,
    [LogOldestNonDistributedVLF] NUMERIC (18)    NULL,
    [LogPctUsedIWatch]           DECIMAL (9, 3)  NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [DBFG_Idx]
    ON [dbo].[Spot_Work_FGInfo]([DBName] ASC, [FGName] ASC) WITH (FILLFACTOR = 50);

