CREATE TABLE [dbo].[EvaluacionIndicadorGestion] (
    [Id_IndicadorGestion] INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto]         INT           NOT NULL,
    [CodConvocatoria]     INT           NOT NULL,
    [Aspecto]             VARCHAR (300) NOT NULL,
    [FechaSeguimiento]    VARCHAR (60)  NOT NULL,
    [Numerador]           VARCHAR (100) NOT NULL,
    [Denominador]         VARCHAR (100) NOT NULL,
    [Descripcion]         VARCHAR (300) NOT NULL,
    [RangoAceptable]      TINYINT       NOT NULL,
    CONSTRAINT [PK_EvaluacionIndicadorGestion] PRIMARY KEY CLUSTERED ([Id_IndicadorGestion] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionIndicadorGestion_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_EvaluacionIndicadorGestion_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

