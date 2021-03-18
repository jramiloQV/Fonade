CREATE TABLE [dbo].[EvaluacionRubroProyecto] (
    [Id_EvaluacionRubroProyecto] INT           IDENTITY (1, 1) NOT NULL,
    [Descripcion]                VARCHAR (255) NULL,
    [CodProyecto]                INT           NULL,
    [CodConvocatoria]            INT           NULL,
    CONSTRAINT [PK_EvaluacionRubroProyecto] PRIMARY KEY CLUSTERED ([Id_EvaluacionRubroProyecto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionRubroProyecto_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_EvaluacionRubroProyecto_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

