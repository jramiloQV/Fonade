CREATE TABLE [dbo].[Spot_Work_TBInfo] (
    [DBName]       [sysname]    NULL,
    [TBID]         INT          NULL,
    [TotalTableKB] NUMERIC (18) NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [DBTBInfoIX_Idx]
    ON [dbo].[Spot_Work_TBInfo]([DBName] ASC, [TBID] ASC) WITH (FILLFACTOR = 50);

