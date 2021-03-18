CREATE TABLE [dbo].[ProyectoCondicionesComerciales] (
    [IdCondicionesComerciales] INT           IDENTITY (1, 1) NOT NULL,
    [IdCliente]                INT           NOT NULL,
    [FrecuenciaCompra]         VARCHAR (MAX) NOT NULL,
    [CaracteristicasCompra]    VARCHAR (MAX) NOT NULL,
    [SitioCompra]              VARCHAR (MAX) NOT NULL,
    [FormaPago]                VARCHAR (MAX) NOT NULL,
    [Precio]                   MONEY         NOT NULL,
    [RequisitosPostVenta]      VARCHAR (MAX) NOT NULL,
    [Garantias]                VARCHAR (MAX) NOT NULL,
    [Margen]                   VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProyectoCondicionesComerciales] PRIMARY KEY CLUSTERED ([IdCondicionesComerciales] ASC),
    CONSTRAINT [FK_ProyectoCondicionesComerciales_ProyectoProtagonistaClientes] FOREIGN KEY ([IdCliente]) REFERENCES [dbo].[ProyectoProtagonistaClientes] ([IdCliente]) ON DELETE CASCADE
);

