CREATE TABLE [dbo].[EvaluacionProyectoSupuestoValor] (
    [CodSupuesto] INT        NOT NULL,
    [Periodo]     INT        NOT NULL,
    [Valor]       FLOAT (53) NULL,
    CONSTRAINT [PK_EvaluacionProyectoSupuestoValor] PRIMARY KEY CLUSTERED ([CodSupuesto] ASC, [Periodo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionProyectoSupuestoValor_EvaluacionProyectoSupuesto] FOREIGN KEY ([CodSupuesto]) REFERENCES [dbo].[EvaluacionProyectoSupuesto] ([Id_EvaluacionProyectoSupuesto])
);

