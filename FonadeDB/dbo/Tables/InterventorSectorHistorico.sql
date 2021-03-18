CREATE TABLE [dbo].[InterventorSectorHistorico] (
    [CodContacto]                   INT         NOT NULL,
    [CodSector]                     INT         NOT NULL,
    [Experiencia]                   VARCHAR (1) NOT NULL,
    [fechaActualizacion]            DATETIME    NULL,
    [Id_InterventorSectorHistorico] INT         IDENTITY (1, 1) NOT NULL,
    [fechaRegistro]                 DATETIME    NULL,
    CONSTRAINT [InterventorSectorHistorico_PK] PRIMARY KEY CLUSTERED ([Id_InterventorSectorHistorico] ASC) WITH (FILLFACTOR = 50)
);

