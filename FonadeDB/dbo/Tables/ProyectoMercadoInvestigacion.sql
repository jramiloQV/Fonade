CREATE TABLE [dbo].[ProyectoMercadoInvestigacion] (
    [CodProyecto]         INT  NOT NULL,
    [AnalisisSector]      TEXT NULL,
    [AnalisisMercado]     TEXT NULL,
    [AnalisisCompetencia] TEXT NULL,
    [Objetivos]           TEXT NULL,
    [Justificacion]       TEXT NULL,
    CONSTRAINT [FK_ProyectoMercadoInvestigacion_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

