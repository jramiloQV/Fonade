CREATE TABLE [dbo].[GrupoObjeto] (
    [CodGrupo]  INT NOT NULL,
    [CodObjeto] INT NOT NULL,
    CONSTRAINT [PK_GrupoObjeto] PRIMARY KEY CLUSTERED ([CodGrupo] ASC, [CodObjeto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_GrupoObjeto_Grupo] FOREIGN KEY ([CodGrupo]) REFERENCES [dbo].[Grupo] ([Id_Grupo]),
    CONSTRAINT [FK_GrupoObjeto_Objeto] FOREIGN KEY ([CodObjeto]) REFERENCES [dbo].[Objeto] ([Id_objeto])
);

