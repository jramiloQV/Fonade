CREATE TABLE [dbo].[Spot_Work_Disks] (
    [DiskName]    VARCHAR (255) NOT NULL,
    [FreeKB]      NUMERIC (18)  NOT NULL,
    [CollectTime] DATETIME      NULL,
    [TotalKB]     NUMERIC (18)  NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [DiskNameIdx]
    ON [dbo].[Spot_Work_Disks]([DiskName] ASC) WITH (FILLFACTOR = 50);

