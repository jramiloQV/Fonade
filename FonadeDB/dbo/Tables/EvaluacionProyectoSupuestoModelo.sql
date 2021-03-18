CREATE TABLE [dbo].[EvaluacionProyectoSupuestoModelo] (
    [NomEvaluacionProyectoSupuesto] VARCHAR (255) NULL,
    [CodTipoSupuesto]               INT           NULL,
    [Inactivo]                      BIT           NULL,
    CONSTRAINT [FK_EvaluacionProyectoSupuestoModelo_TipoSupuesto] FOREIGN KEY ([CodTipoSupuesto]) REFERENCES [dbo].[TipoSupuesto] ([Id_TipoSupuesto])
);

