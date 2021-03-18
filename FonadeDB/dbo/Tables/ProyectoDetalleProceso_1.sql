CREATE TABLE [dbo].[ProyectoDetalleProceso] (
    [IdDetalleProceso]   INT           IDENTITY (1, 1) NOT NULL,
    [IdProducto]         INT           NOT NULL,
    [DescripcionProceso] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProyectoDetalleProceso] PRIMARY KEY CLUSTERED ([IdDetalleProceso] ASC),
    CONSTRAINT [FK_ProyectoDetalleProceso_ProyectoProducto] FOREIGN KEY ([IdProducto]) REFERENCES [dbo].[ProyectoProducto] ([Id_Producto])
);

