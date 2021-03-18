CREATE TABLE [dbo].[Objeto] (
    [Id_objeto]     INT           IDENTITY (1, 1) NOT NULL,
    [CodTipoObjeto] SMALLINT      NOT NULL,
    [NomObjeto]     VARCHAR (80)  NOT NULL,
    [Descripcion]   VARCHAR (255) NOT NULL,
    [Programa]      VARCHAR (80)  NULL,
    [Menu]          BIT           NOT NULL,
    [Orden]         TINYINT       NULL,
    CONSTRAINT [PK_Objeto] PRIMARY KEY CLUSTERED ([Id_objeto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_Objeto_TipoObjeto] FOREIGN KEY ([CodTipoObjeto]) REFERENCES [dbo].[TipoObjeto] ([Id_TipoObjeto])
);

