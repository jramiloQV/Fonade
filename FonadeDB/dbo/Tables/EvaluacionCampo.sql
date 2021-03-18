CREATE TABLE [dbo].[EvaluacionCampo] (
    [codProyecto]     INT      NOT NULL,
    [codConvocatoria] INT      NOT NULL,
    [codCampo]        SMALLINT NOT NULL,
    [Puntaje]         SMALLINT NULL,
    CONSTRAINT [PK_EvaluacionCampo] PRIMARY KEY CLUSTERED ([codProyecto] ASC, [codConvocatoria] ASC, [codCampo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionCampo_Campo] FOREIGN KEY ([codCampo]) REFERENCES [dbo].[Campo] ([id_Campo]),
    CONSTRAINT [FK_EvaluacionCampo_Convocatoria] FOREIGN KEY ([codConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_EvaluacionCampo_Proyecto] FOREIGN KEY ([codProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

