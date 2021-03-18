CREATE TABLE [dbo].[Fog_TraceFilter] (
    [Type]                VARCHAR (15)   NOT NULL,
    [ColumnID]            SMALLINT       NULL,
    [ColumnName]          VARCHAR (30)   NULL,
    [logical_operator]    INT            NULL,
    [comparison_operator] INT            NULL,
    [value_int]           INT            NULL,
    [value_char]          NVARCHAR (255) NULL,
    [id]                  INT            IDENTITY (1, 1) NOT NULL
);

