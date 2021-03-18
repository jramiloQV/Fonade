CREATE TABLE [dbo].[ProyectoProductoInsumo] (
    [CodProducto]  INT          NOT NULL,
    [CodInsumo]    INT          NOT NULL,
    [Presentacion] VARCHAR (30) NULL,
    [Cantidad]     FLOAT (53)   NULL,
    [Desperdicio]  TINYINT      NOT NULL,
    CONSTRAINT [Id_Insumo_pk] PRIMARY KEY CLUSTERED ([CodProducto] ASC, [CodInsumo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoProductoInsumo_ProyectoInsumo] FOREIGN KEY ([CodInsumo]) REFERENCES [dbo].[ProyectoInsumo] ([Id_Insumo]),
    CONSTRAINT [FK_ProyectoProductoInsumo_ProyectoProducto] FOREIGN KEY ([CodProducto]) REFERENCES [dbo].[ProyectoProducto] ([Id_Producto])
);

