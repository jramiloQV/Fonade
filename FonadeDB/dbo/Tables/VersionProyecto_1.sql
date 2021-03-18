CREATE TABLE [dbo].[VersionProyecto] (
    [IdVersionProyecto] INT        IDENTITY (1, 1) NOT NULL,
    [Nombre]            NCHAR (50) NOT NULL,
    CONSTRAINT [PK_TipoTab] PRIMARY KEY CLUSTERED ([IdVersionProyecto] ASC)
);

