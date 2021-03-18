CREATE TABLE [dbo].[NivelEstudio] (
    [Id_NivelEstudio]            INT           IDENTITY (1, 1) NOT NULL,
    [NomNivelEstudio]            VARCHAR (150) NOT NULL,
    [FlagPoseeFechaEtapaLectiva] INT           CONSTRAINT [DF__NivelEstu__FlagP__6423B28F] DEFAULT ((0)) NOT NULL,
    [FlagPoseeFechaFinMateria]   INT           CONSTRAINT [DF__NivelEstu__FlagP__6517D6C8] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_NivelEstudio] PRIMARY KEY CLUSTERED ([Id_NivelEstudio] ASC) WITH (FILLFACTOR = 50)
);

