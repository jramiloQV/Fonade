CREATE TABLE [dbo].[Spot_Work_syscacheobjects] (
    [spid]         INT             CONSTRAINT [DF_Spot_Work_syscacheobjects_spid] DEFAULT (@@spid) NULL,
    [bucketid]     INT             NOT NULL,
    [cacheobjtype] NVARCHAR (17)   NOT NULL,
    [objtype]      NVARCHAR (8)    NOT NULL,
    [objid]        INT             NOT NULL,
    [dbid]         SMALLINT        NOT NULL,
    [uid]          SMALLINT        NOT NULL,
    [refcounts]    INT             NOT NULL,
    [usecounts]    INT             NOT NULL,
    [pagesused]    INT             NOT NULL,
    [setopts]      INT             NOT NULL,
    [langid]       SMALLINT        NOT NULL,
    [dateformat]   SMALLINT        NOT NULL,
    [status]       INT             NOT NULL,
    [sqlbytes]     INT             NOT NULL,
    [sql]          NVARCHAR (2000) NULL,
    [DBName]       [sysname]       NULL,
    [ObjectName]   [sysname]       NULL,
    [UserName]     [sysname]       NULL,
    [id]           INT             IDENTITY (1, 1) NOT NULL,
    [LanguageName] [sysname]       NULL,
    [Options]      VARCHAR (500)   NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [PK_Spot_Work_syscacheobjects]
    ON [dbo].[Spot_Work_syscacheobjects]([id] ASC) WITH (FILLFACTOR = 50);

