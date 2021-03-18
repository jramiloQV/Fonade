CREATE TABLE [dbo].[ContactoEstudioHistorico] (
    [CodContacto]                 INT           NOT NULL,
    [TituloObtenido]              VARCHAR (100) NOT NULL,
    [AnoTitulo]                   SMALLINT      NULL,
    [Institucion]                 VARCHAR (100) NOT NULL,
    [CodCiudad]                   INT           NOT NULL,
    [CodNivelEstudio]             INT           NULL,
    [CodContactoEstudio]          INT           NOT NULL,
    [fechaCreacion]               DATETIME      NULL,
    [fechaActualizacion]          DATETIME      NULL,
    [Id_ContactoEstudioHistorico] INT           IDENTITY (1, 1) NOT NULL,
    [NumActualizacion]            INT           NULL,
    [fechaRegistro]               DATETIME      DEFAULT (getdate()) NOT NULL,
    [FlagIngresadoAsesor]         INT           NULL,
    [CodProgramaAcademico]        INT           NULL,
    [Finalizado]                  INT           NULL,
    [FechaGrado]                  DATETIME      NULL,
    [FechaUltimoCorte]            DATETIME      NULL,
    [SemestresCursados]           INT           NULL,
    [FechaFinMaterias]            DATETIME      NULL,
    CONSTRAINT [PK_ContactoEstudioHsitorico] PRIMARY KEY CLUSTERED ([Id_ContactoEstudioHistorico] ASC) WITH (FILLFACTOR = 50)
);

