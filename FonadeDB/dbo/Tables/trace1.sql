CREATE TABLE [dbo].[trace1] (
    [RowNumber]    INT            IDENTITY (0, 1) NOT NULL,
    [EventClass]   INT            NULL,
    [TextData]     NTEXT          NULL,
    [Duration]     BIGINT         NULL,
    [SPID]         INT            NULL,
    [DatabaseID]   INT            NULL,
    [DatabaseName] NVARCHAR (128) NULL,
    [ObjectType]   INT            NULL,
    [LoginName]    NVARCHAR (128) NULL,
    [BinaryData]   IMAGE          NULL,
    PRIMARY KEY CLUSTERED ([RowNumber] ASC) WITH (FILLFACTOR = 50)
);

