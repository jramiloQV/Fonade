CREATE TABLE [dbo].[Spot_RefreshAutoClose] (
    [DBName]                [sysname]    NOT NULL,
    [DataType]              VARCHAR (20) NOT NULL,
    [PrimDataFileTimestamp] DATETIME     NOT NULL,
    [LastCollectDate]       DATETIME     NOT NULL,
    CONSTRAINT [PK_Spot_RefreshAutoClose] PRIMARY KEY CLUSTERED ([DBName] ASC, [DataType] ASC) WITH (FILLFACTOR = 50)
);

