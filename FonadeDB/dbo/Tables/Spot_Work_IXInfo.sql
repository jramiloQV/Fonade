CREATE TABLE [dbo].[Spot_Work_IXInfo] (
    [CollectTime]    DATETIME      NULL,
    [DBName]         [sysname]     NULL,
    [TBOwner]        [sysname]     NULL,
    [TBName]         [sysname]     NULL,
    [IXName]         [sysname]     NULL,
    [FGName]         [sysname]     NULL,
    [indid]          INT           NULL,
    [TotalTableKB]   NUMERIC (18)  NULL,
    [ReservedKB]     NUMERIC (18)  NULL,
    [UsedKB]         NUMERIC (18)  NULL,
    [IXType]         VARCHAR (50)  NULL,
    [TBOptions]      VARCHAR (255) NULL,
    [TBID]           INT           NULL,
    [TBStatus]       INT           NULL,
    [IXStatus]       INT           NULL,
    [SystemTable]    CHAR (1)      NULL,
    [Pinned]         CHAR (1)      NULL,
    [RefreshTime]    DATETIME      NULL,
    [Rows]           DECIMAL (18)  NULL,
    [DPagesKB]       NUMERIC (18)  NULL,
    [CRDate]         DATETIME      NULL,
    [StatsDate]      DATETIME      NULL,
    [RowModCtr]      INT           NULL,
    [OrigFillFactor] INT           NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [DBTBIX_Idx]
    ON [dbo].[Spot_Work_IXInfo]([DBName] ASC, [TBOwner] ASC, [TBName] ASC, [indid] ASC) WITH (FILLFACTOR = 50);

