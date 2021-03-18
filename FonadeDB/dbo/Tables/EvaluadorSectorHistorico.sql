CREATE TABLE [dbo].[EvaluadorSectorHistorico] (
    [CodContacto]                 INT         NOT NULL,
    [CodSector]                   INT         NOT NULL,
    [Experiencia]                 VARCHAR (1) NOT NULL,
    [fechaActualizacion]          DATETIME    NULL,
    [Id_EvaluadorSectorHistorico] INT         IDENTITY (1, 1) NOT NULL,
    [fechaRegistro]               DATETIME    NULL,
    CONSTRAINT [EvaluadorSectorHistorico_PK] PRIMARY KEY CLUSTERED ([Id_EvaluadorSectorHistorico] ASC) WITH (FILLFACTOR = 50)
);

