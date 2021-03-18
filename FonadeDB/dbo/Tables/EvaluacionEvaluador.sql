CREATE TABLE [dbo].[EvaluacionEvaluador] (
    [CodProyecto]     INT      NOT NULL,
    [CodConvocatoria] INT      NOT NULL,
    [CodItem]         INT      NOT NULL,
    [Puntaje]         SMALLINT NULL,
    CONSTRAINT [PK_EvaluacionEvaluador] PRIMARY KEY CLUSTERED ([CodProyecto] ASC, [CodConvocatoria] ASC, [CodItem] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionEvaluador_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_EvaluacionEvaluador_Item] FOREIGN KEY ([CodItem]) REFERENCES [dbo].[Item] ([Id_Item]),
    CONSTRAINT [FK_EvaluacionEvaluador_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

