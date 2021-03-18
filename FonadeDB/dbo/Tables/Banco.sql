CREATE TABLE [dbo].[Banco] (
    [Id_Banco] INT          IDENTITY (1, 1) NOT NULL,
    [nomBanco] VARCHAR (50) NOT NULL,
    [codigo]   VARCHAR (5)  NOT NULL,
    CONSTRAINT [Banco_PK] PRIMARY KEY CLUSTERED ([Id_Banco] ASC) WITH (FILLFACTOR = 50)
);

