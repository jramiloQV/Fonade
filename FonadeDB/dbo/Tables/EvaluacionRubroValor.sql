CREATE TABLE [dbo].[EvaluacionRubroValor] (
    [CodEvaluacionRubroProyecto] INT        NOT NULL,
    [Periodo]                    INT        NOT NULL,
    [Valor]                      FLOAT (53) NULL,
    CONSTRAINT [PK_EvaluacionRubroValor] PRIMARY KEY CLUSTERED ([CodEvaluacionRubroProyecto] ASC, [Periodo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionRubroValor_EvaluacionRubroProyecto] FOREIGN KEY ([CodEvaluacionRubroProyecto]) REFERENCES [dbo].[EvaluacionRubroProyecto] ([Id_EvaluacionRubroProyecto])
);

