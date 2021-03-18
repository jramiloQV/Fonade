CREATE TABLE [dbo].[ProyectoResumenEjecutivo] (
    [CodProyecto]             INT  NOT NULL,
    [ConceptoNegocio]         TEXT NULL,
    [PotencialMercados]       TEXT NULL,
    [VentajasCompetitivas]    TEXT NULL,
    [ResumenInversiones]      TEXT NULL,
    [Proyecciones]            TEXT NULL,
    [ConclusionesFinancieras] TEXT NULL,
    CONSTRAINT [FK_ProyectoResumenEjecutivo_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

