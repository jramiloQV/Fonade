CREATE TABLE [dbo].[ProyectoMercadoProyeccionVentas] (
    [Id_ProyectoProyeccionVentas] INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto]                 INT           NOT NULL,
    [FechaArranque]               SMALLDATETIME NULL,
    [CodPeriodo]                  TINYINT       NULL,
    [TiempoProyeccion]            TINYINT       NULL,
    [MetodoProyeccion]            VARCHAR (100) NULL,
    [PoliticaCartera]             TEXT          NULL,
    [CostoVenta]                  VARCHAR (100) NULL,
    [justificacion]               TEXT          NULL,
    CONSTRAINT [PK_ProyectoMercadoProyeccionVentas] PRIMARY KEY CLUSTERED ([Id_ProyectoProyeccionVentas] ASC),
    CONSTRAINT [FK_ProyectoMercadoProyeccionVentas_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);





