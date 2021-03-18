CREATE TABLE [dbo].[Tab] (
    [Id_Tab]            SMALLINT     IDENTITY (1, 1) NOT NULL,
    [NomTab]            VARCHAR (50) NOT NULL,
    [CodTab]            SMALLINT     NULL,
    [IdVersionProyecto] INT          CONSTRAINT [DF_Tab_Id_TipoTab] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_SubTab] PRIMARY KEY CLUSTERED ([Id_Tab] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_Tab_Tab] FOREIGN KEY ([CodTab]) REFERENCES [dbo].[Tab] ([Id_Tab]),
    CONSTRAINT [FK_Tab_VersionProyecto] FOREIGN KEY ([IdVersionProyecto]) REFERENCES [dbo].[VersionProyecto] ([IdVersionProyecto])
);



