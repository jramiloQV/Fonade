CREATE TABLE [dbo].[TipoInsumo] (
    [Id_TipoInsumo] TINYINT      IDENTITY (1, 1) NOT NULL,
    [NomTipoInsumo] VARCHAR (60) NOT NULL,
    CONSTRAINT [PK_TipoInsumo] PRIMARY KEY CLUSTERED ([Id_TipoInsumo] ASC) WITH (FILLFACTOR = 50)
);

