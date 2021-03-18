CREATE TABLE [dbo].[ProyectoDesarrolloSolucion] (
    [IdDesarrolloSolucion]  INT           IDENTITY (1, 1) NOT NULL,
    [IdProyecto]            INT           NOT NULL,
    [Ingresos]              VARCHAR (MAX) NOT NULL,
    [DondeCompra]           VARCHAR (MAX) NULL,
    [CaracteristicasCompra] VARCHAR (MAX) NULL,
    [FrecuenciaCompra]      VARCHAR (MAX) NULL,
    [Precio]                VARCHAR (MAX) NULL,
    CONSTRAINT [PK_ProyectoDesarrolloSolucion] PRIMARY KEY CLUSTERED ([IdDesarrolloSolucion] ASC),
    CONSTRAINT [FK_ProyectoDesarrolloSolucion_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

