CREATE TABLE [dbo].[ProyectoInsumo] (
    [Id_Insumo]     INT              IDENTITY (1, 1) NOT NULL,
    [CodProyecto]   INT              NOT NULL,
    [codTipoInsumo] TINYINT          NOT NULL,
    [nomInsumo]     VARCHAR (100)    NOT NULL,
    [IVA]           FLOAT (53)       NOT NULL,
    [Unidad]        VARCHAR (15)     NULL,
    [Presentacion]  VARCHAR (30)     NULL,
    [CompraContado] FLOAT (53)       NOT NULL,
    [CompraCredito] FLOAT (53)       NOT NULL,
    [VersionId]     UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_ProyectoInsumo] PRIMARY KEY CLUSTERED ([Id_Insumo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoInsumo_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto]),
    CONSTRAINT [FK_ProyectoInsumo_TipoInsumo] FOREIGN KEY ([codTipoInsumo]) REFERENCES [dbo].[TipoInsumo] ([Id_TipoInsumo])
);

