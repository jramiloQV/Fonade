CREATE TABLE [dbo].[EvaluacionRiesgo] (
    [Id_Riesgo]       INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto]     INT           NOT NULL,
    [CodConvocatoria] INT           NOT NULL,
    [Riesgo]          VARCHAR (500) NOT NULL,
    [Mitigacion]      VARCHAR (500) NOT NULL,
    CONSTRAINT [PK_EvaluacionRiesgo] PRIMARY KEY CLUSTERED ([Id_Riesgo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionRiesgo_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_EvaluacionRiesgo_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

