CREATE TABLE [dbo].[Spot_Work_BufferCacheContents] (
    [id]                    INT            IDENTITY (1, 1) NOT NULL,
    [dbid]                  INT            NULL,
    [objectid]              INT            NULL,
    [indid]                 INT            NULL,
    [buffers]               NUMERIC (18)   NULL,
    [DBName]                [sysname]      NULL,
    [TBName]                [sysname]      NULL,
    [IXName]                [sysname]      NULL,
    [SizeInCacheKB]         NUMERIC (18)   NULL,
    [Pinned]                CHAR (1)       NULL,
    [TBOwner]               [sysname]      NULL,
    [PercentageOfCache]     DECIMAL (5, 2) NULL,
    [ObjectSizeKB]          NUMERIC (18)   NULL,
    [PercentageOfObject]    DECIMAL (5, 2) NULL,
    [FileGroup]             [sysname]      NULL,
    [DirtyBuffers]          NUMERIC (18)   NULL,
    [PercentageObjectDirty] DECIMAL (5, 2) NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [ID_Idx]
    ON [dbo].[Spot_Work_BufferCacheContents]([id] ASC) WITH (FILLFACTOR = 50);

