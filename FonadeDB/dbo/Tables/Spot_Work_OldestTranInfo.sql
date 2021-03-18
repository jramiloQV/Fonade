CREATE TABLE [dbo].[Spot_Work_OldestTranInfo] (
    [DBName]            [sysname]    NOT NULL,
    [OldestTranTime]    DATETIME     NULL,
    [OldestTranSpid]    INT          NULL,
    [OldestTranLSN]     VARCHAR (20) NULL,
    [OldestTranVLF]     NUMERIC (18) NULL,
    [ReplDistOldLSN]    VARCHAR (20) NULL,
    [ReplDistOldVLF]    NUMERIC (18) NULL,
    [ReplNonDistOldLSN] VARCHAR (20) NULL,
    [ReplNonDistOldVLF] NUMERIC (18) NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [PK_Spot_Work_OldestTranInfo]
    ON [dbo].[Spot_Work_OldestTranInfo]([DBName] ASC) WITH (FILLFACTOR = 50);

