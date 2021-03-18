CREATE TABLE [dbo].[EvaluacionIndicadorFinancieroValor] (
    [CodEvaluacionIndicadorFinancieroProyecto] INT        NOT NULL,
    [Periodo]                                  INT        NOT NULL,
    [Valor]                                    FLOAT (53) NULL,
    CONSTRAINT [PK_EvaluacionIndicadorFinancieroValor] PRIMARY KEY CLUSTERED ([CodEvaluacionIndicadorFinancieroProyecto] ASC, [Periodo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionIndicadorFinancieroValor_EvaluacionIndicadorFinancieroProyecto] FOREIGN KEY ([CodEvaluacionIndicadorFinancieroProyecto]) REFERENCES [dbo].[EvaluacionIndicadorFinancieroProyecto] ([Id_EvaluacionIndicadorFinancieroProyecto])
);

