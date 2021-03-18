CREATE TABLE [dbo].[TipoInfraestructura] (
    [Id_TipoInfraestructura] TINYINT      IDENTITY (1, 1) NOT NULL,
    [NomTipoInfraestructura] VARCHAR (60) NOT NULL,
    [IdVersion]              INT          NULL,
    CONSTRAINT [PK_TipoInfraestructura] PRIMARY KEY CLUSTERED ([Id_TipoInfraestructura] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_TipoInfraestructura_VersionProyecto] FOREIGN KEY ([IdVersion]) REFERENCES [dbo].[VersionProyecto] ([IdVersionProyecto])
);



