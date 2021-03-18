CREATE TABLE [dbo].[TipoObjeto] (
    [Id_TipoObjeto] SMALLINT     IDENTITY (1, 1) NOT NULL,
    [NomTipoObjeto] VARCHAR (80) NOT NULL,
    [Orden]         TINYINT      NULL,
    CONSTRAINT [PK_TipoObjeto] PRIMARY KEY CLUSTERED ([Id_TipoObjeto] ASC) WITH (FILLFACTOR = 50)
);

