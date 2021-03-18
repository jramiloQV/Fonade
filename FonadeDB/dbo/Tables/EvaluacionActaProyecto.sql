CREATE TABLE [dbo].[EvaluacionActaProyecto] (
    [CodActa]     INT NOT NULL,
    [CodProyecto] INT NOT NULL,
    [Viable]      BIT NOT NULL,
    CONSTRAINT [PK_EvaluacionActaProyecto] PRIMARY KEY CLUSTERED ([CodActa] ASC, [CodProyecto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionActaProyecto_EvaluacionActa] FOREIGN KEY ([CodActa]) REFERENCES [dbo].[EvaluacionActa] ([Id_Acta]),
    CONSTRAINT [FK_EvaluacionActaProyecto_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

