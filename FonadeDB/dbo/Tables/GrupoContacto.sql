CREATE TABLE [dbo].[GrupoContacto] (
    [CodGrupo]    INT NOT NULL,
    [CodContacto] INT NOT NULL,
    CONSTRAINT [PK_GrupoContacto] PRIMARY KEY CLUSTERED ([CodGrupo] ASC, [CodContacto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_GrupoContacto_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_GrupoContacto_Grupo] FOREIGN KEY ([CodGrupo]) REFERENCES [dbo].[Grupo] ([Id_Grupo])
);


GO
CREATE NONCLUSTERED INDEX [ID_GrupoContacto_CodContacto]
    ON [dbo].[GrupoContacto]([CodContacto] ASC) WITH (FILLFACTOR = 50);

