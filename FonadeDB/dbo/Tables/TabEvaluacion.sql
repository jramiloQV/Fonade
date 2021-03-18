CREATE TABLE [dbo].[TabEvaluacion] (
    [Id_TabEvaluacion]  SMALLINT     IDENTITY (1, 1) NOT NULL,
    [NomTabEvaluacion]  VARCHAR (50) NOT NULL,
    [CodTabEvaluacion]  SMALLINT     NULL,
    [IdVersionProyecto] INT          NULL,
    CONSTRAINT [PK_TabEvaluacion] PRIMARY KEY CLUSTERED ([Id_TabEvaluacion] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_TabEvaluacion_TabEvaluacion] FOREIGN KEY ([CodTabEvaluacion]) REFERENCES [dbo].[TabEvaluacion] ([Id_TabEvaluacion]),
    CONSTRAINT [FK_TabEvaluacion_VersionProyecto] FOREIGN KEY ([IdVersionProyecto]) REFERENCES [dbo].[VersionProyecto] ([IdVersionProyecto])
);



