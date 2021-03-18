CREATE TABLE [dbo].[ProyectoOperacion] (
    [CodProyecto]        INT  NOT NULL,
    [FichaProducto]      TEXT NULL,
    [EstadoDesarrollo]   TEXT NULL,
    [DescripcionProceso] TEXT NULL,
    [Necesidades]        TEXT NULL,
    [PlanProduccion]     TEXT NULL,
    CONSTRAINT [FK_ProyectoOperacion_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

