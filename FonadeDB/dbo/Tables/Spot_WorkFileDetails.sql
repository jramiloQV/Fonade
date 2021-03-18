CREATE TABLE [dbo].[Spot_WorkFileDetails] (
    [spid]           INT          CONSTRAINT [DF_Spot_WorkFileDetails_spid] DEFAULT (@@spid) NULL,
    [AlternateName]  VARCHAR (50) NULL,
    [FileSize]       NUMERIC (18) NULL,
    [CreationDate]   VARCHAR (8)  NULL,
    [CreationTime]   VARCHAR (6)  NULL,
    [LastWriteDate]  VARCHAR (8)  NULL,
    [LastWriteTime]  VARCHAR (6)  NULL,
    [LastAccessDate] VARCHAR (8)  NULL,
    [LastAccessTime] VARCHAR (6)  NULL,
    [Attributes]     VARCHAR (10) NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [spidFileNameIdx]
    ON [dbo].[Spot_WorkFileDetails]([spid] ASC, [AlternateName] ASC) WITH (FILLFACTOR = 50);

