CREATE TABLE [dbo].[EvaluacionProyectoSupuesto] (
    [Id_EvaluacionProyectoSupuesto] INT           IDENTITY (1, 1) NOT NULL,
    [NomEvaluacionProyectoSupuesto] VARCHAR (255) NULL,
    [CodTipoSupuesto]               INT           NOT NULL,
    [CodProyecto]                   INT           NULL,
    [CodConvocatoria]               INT           NULL,
    CONSTRAINT [PK_EvaluacionProyectoSupuesto] PRIMARY KEY CLUSTERED ([Id_EvaluacionProyectoSupuesto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionProyectoSupuesto_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_EvaluacionProyectoSupuesto_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto]),
    CONSTRAINT [FK_EvaluacionProyectoSupuesto_TipoSupuesto] FOREIGN KEY ([CodTipoSupuesto]) REFERENCES [dbo].[TipoSupuesto] ([Id_TipoSupuesto])
);

