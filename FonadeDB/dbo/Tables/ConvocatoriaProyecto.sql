CREATE TABLE [dbo].[ConvocatoriaProyecto] (
    [CodConvocatoria]        INT           NOT NULL,
    [CodProyecto]            INT           NOT NULL,
    [Fecha]                  SMALLDATETIME NOT NULL,
    [Justificacion]          TEXT          NULL,
    [Viable]                 BIT           NULL,
    [codevaluacionconceptos] INT           NULL,
    CONSTRAINT [PK_ConvocatoriaProyecto] PRIMARY KEY CLUSTERED ([CodConvocatoria] ASC, [CodProyecto] ASC, [Fecha] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ConvocatoriaProyecto_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_ConvocatoriaProyecto_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);


GO
CREATE NONCLUSTERED INDEX [Idx_Convocatoria]
    ON [dbo].[ConvocatoriaProyecto]([CodProyecto] ASC)
    INCLUDE([CodConvocatoria]) WITH (FILLFACTOR = 50);

