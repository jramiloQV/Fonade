CREATE TABLE [dbo].[TipoFinanciacion] (
    [Id_TipoFinanciacion] TINYINT      IDENTITY (1, 1) NOT NULL,
    [NomTipoFinanciacion] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_TipoFinanciacion] PRIMARY KEY CLUSTERED ([Id_TipoFinanciacion] ASC) WITH (FILLFACTOR = 50)
);

