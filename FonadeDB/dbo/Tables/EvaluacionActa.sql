CREATE TABLE [dbo].[EvaluacionActa] (
    [Id_Acta]         INT            IDENTITY (1, 1) NOT NULL,
    [NomActa]         VARCHAR (80)   NOT NULL,
    [NumActa]         VARCHAR (10)   NOT NULL,
    [FechaActa]       SMALLDATETIME  NOT NULL,
    [Observaciones]   VARCHAR (1500) NULL,
    [CodConvocatoria] INT            NULL,
    [publicado]       BIT            NULL,
    CONSTRAINT [PK_EvaluacionActa] PRIMARY KEY CLUSTERED ([Id_Acta] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionActa_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria])
);

