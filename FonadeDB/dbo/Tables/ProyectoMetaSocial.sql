CREATE TABLE [dbo].[ProyectoMetaSocial] (
    [CodProyecto]          INT           NOT NULL,
    [PlanNacional]         VARCHAR (500) NOT NULL,
    [PlanRegional]         VARCHAR (500) NOT NULL,
    [Cluster]              VARCHAR (500) NOT NULL,
    [EmpleoDirecto]        SMALLINT      NULL,
    [EmpleoPrimerAno]      SMALLINT      NULL,
    [Empleo18a24]          SMALLINT      NULL,
    [EmpleoDesplazados]    SMALLINT      NULL,
    [EmpleoMadres]         SMALLINT      NULL,
    [EmpleoMinorias]       SMALLINT      NULL,
    [EmpleoRecluidos]      SMALLINT      NULL,
    [EmpleoDesmovilizados] SMALLINT      NULL,
    [EmpleoDiscapacitados] SMALLINT      NULL,
    [EmpleoDesvinculados]  SMALLINT      NULL,
    [EmpleoIndirecto]      SMALLINT      NULL,
    CONSTRAINT [FK_ProyectoMetaSocial] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

