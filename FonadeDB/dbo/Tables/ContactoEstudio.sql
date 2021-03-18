CREATE TABLE [dbo].[ContactoEstudio] (
    [CodContacto]          INT           NOT NULL,
    [TituloObtenido]       VARCHAR (500) NOT NULL,
    [AnoTitulo]            SMALLINT      NULL,
    [Institucion]          VARCHAR (500) NOT NULL,
    [CodCiudad]            INT           NOT NULL,
    [CodNivelEstudio]      INT           NULL,
    [Id_ContactoEstudio]   INT           IDENTITY (1, 1) NOT NULL,
    [FlagIngresadoAsesor]  INT           CONSTRAINT [DF__ContactoE__FlagI__5F5EFD72] DEFAULT ((0)) NULL,
    [CodProgramaAcademico] INT           NULL,
    [Finalizado]           INT           NULL,
    [FechaGrado]           DATETIME      NULL,
    [FechaUltimoCorte]     DATETIME      NULL,
    [SemestresCursados]    INT           NULL,
    [FechaInicio]          DATETIME      NULL,
    [fechaCreacion]        DATETIME      NULL,
    [fechaActualizacion]   DATETIME      NULL,
    [FechaFinMaterias]     DATETIME      NULL,
    CONSTRAINT [PK_ContactoEstudio] PRIMARY KEY CLUSTERED ([Id_ContactoEstudio] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ContactoEstudio_Ciudad] FOREIGN KEY ([CodCiudad]) REFERENCES [dbo].[Ciudad] ([Id_Ciudad]),
    CONSTRAINT [FK_ContactoEstudio_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_ContactoEstudio_NivelEstudio] FOREIGN KEY ([CodNivelEstudio]) REFERENCES [dbo].[NivelEstudio] ([Id_NivelEstudio])
);

