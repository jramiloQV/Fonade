CREATE TABLE [dbo].[TabEvaluacionProyecto] (
    [CodTabEvaluacion]  SMALLINT NOT NULL,
    [CodProyecto]       INT      NOT NULL,
    [CodConvocatoria]   INT      NOT NULL,
    [CodContacto]       INT      NOT NULL,
    [FechaModificacion] DATETIME NOT NULL,
    [Realizado]         BIT      CONSTRAINT [DF__TabEvalua__Reali__3508D0F3] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_TabEvaluacionProyecto] PRIMARY KEY CLUSTERED ([CodTabEvaluacion] ASC, [CodProyecto] ASC, [CodConvocatoria] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_TabEvaluacionProyecto_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_TabEvaluacionProyecto_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_TabEvaluacionProyecto_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto]),
    CONSTRAINT [FK_TabEvaluacionProyecto_TabEvaluacion] FOREIGN KEY ([CodTabEvaluacion]) REFERENCES [dbo].[TabEvaluacion] ([Id_TabEvaluacion])
);

