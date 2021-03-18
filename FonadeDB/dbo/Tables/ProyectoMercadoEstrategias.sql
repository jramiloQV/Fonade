CREATE TABLE [dbo].[ProyectoMercadoEstrategias] (
    [CodProyecto]                  INT  NOT NULL,
    [ConceptoProducto]             TEXT NULL,
    [EstrategiasDistribucion]      TEXT NULL,
    [EstrategiasPrecio]            TEXT NULL,
    [EstrategiasPromocion]         TEXT NULL,
    [EstrategiasComunicacion]      TEXT NULL,
    [EstrategiasServicio]          TEXT NULL,
    [PresupuestoMercado]           TEXT NULL,
    [EstrategiasAprovisionamiento] TEXT NULL,
    CONSTRAINT [FK_ProyectoMercadoEstrategias_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

