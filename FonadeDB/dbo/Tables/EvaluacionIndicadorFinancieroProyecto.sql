CREATE TABLE [dbo].[EvaluacionIndicadorFinancieroProyecto] (
    [Id_EvaluacionIndicadorFinancieroProyecto] INT           IDENTITY (1, 1) NOT NULL,
    [Descripcion]                              VARCHAR (255) NOT NULL,
    [CodProyecto]                              INT           NOT NULL,
    [CodConvocatoria]                          INT           NOT NULL,
    CONSTRAINT [PK_EvaluacionIndicadoresFinancierosProyecto] PRIMARY KEY CLUSTERED ([Id_EvaluacionIndicadorFinancieroProyecto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionIndicadoresFinancierosProyecto_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_EvaluacionIndicadoresFinancierosProyecto_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

