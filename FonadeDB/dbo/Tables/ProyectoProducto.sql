CREATE TABLE [dbo].[ProyectoProducto] (
    [Id_Producto]             INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto]             INT           NOT NULL,
    [NomProducto]             VARCHAR (255) NULL,
    [PorcentajeIva]           FLOAT (53)    NULL,
    [PorcentajeRetencion]     FLOAT (53)    NULL,
    [PorcentajeVentasContado] FLOAT (53)    NULL,
    [PorcentajeVentasPlazo]   FLOAT (53)    NULL,
    [PosicionArancelaria]     CHAR (10)     NULL,
    [PrecioLanzamiento]       MONEY         NULL,
    [NombreComercial]         VARCHAR (255) NULL,
    [UnidadMedida]            VARCHAR (255) NULL,
    [DescripcionGeneral]      VARCHAR (255) NULL,
    [CondicionesEspeciales]   VARCHAR (255) NULL,
    [Composicion]             VARCHAR (255) NULL,
    [Otros]                   VARCHAR (255) NULL,
    [FormaDePago]             VARCHAR (250) NULL,
    [Justificacion]           VARCHAR (250) NULL,
    [Iva]                     INT           NULL,
    CONSTRAINT [PK_ProyectoProducto] PRIMARY KEY CLUSTERED ([Id_Producto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoProducto_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);



