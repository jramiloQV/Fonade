CREATE TABLE [dbo].[ProyectoProductoUnidadesVentas] (
    [id_proyectoUnidadesVentas] INT        IDENTITY (1, 1) NOT NULL,
    [CodProducto]               INT        NOT NULL,
    [Unidades]                  FLOAT (53) NULL,
    [Mes]                       SMALLINT   NOT NULL,
    [Ano]                       SMALLINT   NOT NULL,
    CONSTRAINT [PK_ProyectoProductoUnidadesVentas] PRIMARY KEY NONCLUSTERED ([id_proyectoUnidadesVentas] ASC),
    CONSTRAINT [FK_ProyectoProductoUnidadesVentas_ProyectoProducto] FOREIGN KEY ([CodProducto]) REFERENCES [dbo].[ProyectoProducto] ([Id_Producto])
);




GO
CREATE CLUSTERED INDEX [IDX_Producto]
    ON [dbo].[ProyectoProductoUnidadesVentas]([CodProducto] ASC) WITH (FILLFACTOR = 50, ALLOW_PAGE_LOCKS = OFF);

