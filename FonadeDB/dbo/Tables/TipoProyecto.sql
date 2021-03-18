CREATE TABLE [dbo].[TipoProyecto] (
    [Id_TipoProyecto] SMALLINT      IDENTITY (1, 1) NOT NULL,
    [NomTipoProyecto] VARCHAR (80)  NOT NULL,
    [Descripcion]     VARCHAR (128) NULL,
    CONSTRAINT [PK_TipoProyecto] PRIMARY KEY CLUSTERED ([Id_TipoProyecto] ASC) WITH (FILLFACTOR = 50)
);

