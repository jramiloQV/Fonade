CREATE TABLE [dbo].[ConvocatoriaReglaSalarios] (
    [CodConvocatoria]   INT         NOT NULL,
    [ExpresionLogica]   VARCHAR (5) NOT NULL,
    [EmpleosGenerados1] INT         NOT NULL,
    [EmpleosGenerados2] INT         NULL,
    [SalariosAPrestar]  INT         NOT NULL,
    [NoRegla]           INT         NOT NULL,
    CONSTRAINT [FK_ConvocatoriaReglaSalarios_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria])
);

