CREATE TABLE [dbo].[Audit] (
    [AuditID]         INT            IDENTITY (1, 1) NOT NULL,
    [Type]            CHAR (1)       NULL,
    [TableName]       VARCHAR (128)  NULL,
    [PrimaryKeyField] VARCHAR (1000) NULL,
    [PrimaryKeyValue] VARCHAR (1000) NULL,
    [FieldName]       VARCHAR (128)  NULL,
    [OldValue]        VARCHAR (1000) NULL,
    [NewValue]        VARCHAR (1000) NULL,
    [UpdateDate]      DATETIME       DEFAULT (getdate()) NULL,
    [UserName]        VARCHAR (128)  NULL,
    [IpUser]          VARCHAR (128)  NULL
);

