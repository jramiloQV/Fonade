CREATE TABLE [dbo].[ProgramaAcademico] (
    [Id_ProgramaAcademico]    INT           NOT NULL,
    [NomProgramaAcademico]    VARCHAR (350) NOT NULL,
    [Nombre]                  VARCHAR (100) NOT NULL,
    [CodInstitucionEducativa] INT           NOT NULL,
    [Estado]                  VARCHAR (50)  NOT NULL,
    [Metodologia]             VARCHAR (50)  NOT NULL,
    [NomMunicipio]            VARCHAR (200) NOT NULL,
    [NomDepartamento]         VARCHAR (200) NULL,
    [CodNivelEstudio]         INT           NOT NULL,
    [CodCiudad]               INT           NOT NULL,
    CONSTRAINT [PK_ProgramaAcademico] PRIMARY KEY CLUSTERED ([Id_ProgramaAcademico] ASC) WITH (FILLFACTOR = 50)
);

