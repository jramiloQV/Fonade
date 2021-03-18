CREATE TABLE [dbo].[ActaAcreditacionFinal] (
    [Id_ActaAcreditacionFinal] INT           IDENTITY (1, 1) NOT NULL,
    [NomActaAcreditacionFinal] VARCHAR (100) NULL,
    [CodConvocatoria]          INT           NOT NULL,
    [FechaCreacion]            DATETIME      NULL,
    [FechaTransmision]         DATETIME      NULL
);

