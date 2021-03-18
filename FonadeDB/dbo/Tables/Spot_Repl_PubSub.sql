CREATE TABLE [dbo].[Spot_Repl_PubSub] (
    [RowType]         VARCHAR (15)     NULL,
    [Type]            VARCHAR (15)     NULL,
    [Name]            [sysname]        NULL,
    [Server]          [sysname]        NULL,
    [DBName]          [sysname]        NULL,
    [Status]          VARCHAR (15)     NULL,
    [Conflicts]       NUMERIC (18)     NULL,
    [JobID]           UNIQUEIDENTIFIER NULL,
    [PublicationName] [sysname]        NULL,
    [Publisher]       [sysname]        NULL,
    [PublicationDB]   [sysname]        NULL,
    [Description]     VARCHAR (255)    NULL,
    [ParentID]        INT              NULL,
    [id]              INT              IDENTITY (1, 1) NOT NULL,
    [NumericData1]    NUMERIC (18)     NULL,
    [StatusText]      VARCHAR (20)     NULL,
    [Icon]            VARCHAR (10)     NULL,
    [IconSeverity]    SMALLINT         NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [PK_Spot_Repl_PubSub]
    ON [dbo].[Spot_Repl_PubSub]([id] ASC) WITH (FILLFACTOR = 50);

