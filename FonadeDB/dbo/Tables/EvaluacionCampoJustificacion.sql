CREATE TABLE [dbo].[EvaluacionCampoJustificacion] (
    [CodProyecto]     INT            NOT NULL,
    [CodConvocatoria] INT            NOT NULL,
    [CodCampo]        SMALLINT       NOT NULL,
    [Justificacion]   VARCHAR (1000) NULL,
    CONSTRAINT [PK_EvaluacionCampoJustificacion] PRIMARY KEY CLUSTERED ([CodProyecto] ASC, [CodConvocatoria] ASC, [CodCampo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionCampoJustificacion_Campo] FOREIGN KEY ([CodCampo]) REFERENCES [dbo].[Campo] ([id_Campo]),
    CONSTRAINT [FK_EvaluacionCampoJustificacion_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_EvaluacionCampoJustificacion_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

