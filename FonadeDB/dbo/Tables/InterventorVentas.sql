CREATE TABLE [dbo].[InterventorVentas] (
    [id_ventas]   INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto] INT           NOT NULL,
    [NomProducto] VARCHAR (100) NULL,
    CONSTRAINT [PK_InterventorVentas] PRIMARY KEY CLUSTERED ([id_ventas] ASC)
);

