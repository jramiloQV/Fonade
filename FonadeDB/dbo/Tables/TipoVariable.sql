CREATE TABLE [dbo].[TipoVariable] (
    [Id_TipoVariable] INT           IDENTITY (1, 1) NOT NULL,
    [NomTipoVariable] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_TipoVariable] PRIMARY KEY CLUSTERED ([Id_TipoVariable] ASC) WITH (FILLFACTOR = 50)
);

