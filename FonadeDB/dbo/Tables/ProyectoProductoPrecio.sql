CREATE TABLE [dbo].[ProyectoProductoPrecio] (
    [CodProducto] INT       NOT NULL,
    [Periodo]     TINYINT   NOT NULL,
    [Precio]      CHAR (10) NULL,
    [Valor]       MONEY     NULL,
    CONSTRAINT [PK_ProyectoProductoPrecio] PRIMARY KEY CLUSTERED ([CodProducto] ASC, [Periodo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoProductoPrecio_ProyectoProducto] FOREIGN KEY ([CodProducto]) REFERENCES [dbo].[ProyectoProducto] ([Id_Producto])
);



