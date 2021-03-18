CREATE TABLE [dbo].[TipoSupuesto] (
    [Id_TipoSupuesto] INT       IDENTITY (1, 1) NOT NULL,
    [NomTipoSupuesto] CHAR (30) NULL,
    CONSTRAINT [PK_TipoSupuesto] PRIMARY KEY CLUSTERED ([Id_TipoSupuesto] ASC) WITH (FILLFACTOR = 50)
);

