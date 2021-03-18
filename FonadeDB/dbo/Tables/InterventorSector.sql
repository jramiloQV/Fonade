CREATE TABLE [dbo].[InterventorSector] (
    [CodContacto]        INT         NOT NULL,
    [CodSector]          INT         NOT NULL,
    [Experiencia]        VARCHAR (1) CONSTRAINT [DF_InterventorSector_Experiencia] DEFAULT ('A') NOT NULL,
    [fechaActualizacion] DATETIME    DEFAULT (getdate()) NOT NULL
);

