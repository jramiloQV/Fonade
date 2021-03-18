CREATE TABLE [dbo].[Spot_Work_FileInfo] (
    [CollectTime]              DATETIME        NULL,
    [DBName]                   [sysname]       NULL,
    [FileName]                 [sysname]       NULL,
    [FileGroupName]            [sysname]       NULL,
    [FileSizeKB]               NUMERIC (18)    NULL,
    [FileMaxSizeKB]            NUMERIC (18)    NULL,
    [FileGrowthIncrement]      VARCHAR (20)    NULL,
    [FileType]                 VARCHAR (20)    NULL,
    [FileDisk]                 VARCHAR (255)   NULL,
    [FilePath]                 VARCHAR (255)   NULL,
    [FileID]                   SMALLINT        NULL,
    [FileMaxSizeChar]          VARCHAR (20)    NULL,
    [FileSizeChar]             VARCHAR (20)    NULL,
    [PotentialGrowthKB]        NUMERIC (18)    NULL,
    [DiskFreeKB]               NUMERIC (18)    NULL,
    [CanGrow]                  CHAR (1)        NULL,
    [PotentialGrowthPct]       NUMERIC (16, 2) NULL,
    [FileGroupID]              INT             NULL,
    [FileStatus]               INT             NULL,
    [Usage]                    VARCHAR (20)    NULL,
    [FileGrowthIncrementNum]   NUMERIC (18)    NULL,
    [FileUsedKB]               NUMERIC (18)    NULL,
    [MasterSysaltfilesSize]    INT             NULL,
    [MasterSysaltfilesMaxSize] INT             NULL,
    [MasterSysaltfilesGrowth]  INT             NULL,
    [MasterSysaltfilesStatus]  INT             NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [DBFile_Idx]
    ON [dbo].[Spot_Work_FileInfo]([DBName] ASC, [FileName] ASC) WITH (FILLFACTOR = 50);

