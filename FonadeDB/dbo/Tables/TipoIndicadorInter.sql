CREATE TABLE [dbo].[TipoIndicadorInter] (
    [Id_TipoIndicadorInter] TINYINT      IDENTITY (1, 1) NOT NULL,
    [NomTipoIndicadorInter] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_TipoIndicadorInter] PRIMARY KEY CLUSTERED ([Id_TipoIndicadorInter] ASC) WITH (FILLFACTOR = 50)
);

