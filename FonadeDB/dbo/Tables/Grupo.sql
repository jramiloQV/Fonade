CREATE TABLE [dbo].[Grupo] (
    [Id_Grupo]    INT           IDENTITY (1, 1) NOT NULL,
    [NomGrupo]    VARCHAR (100) NOT NULL,
    [Descripcion] VARCHAR (255) NULL,
    CONSTRAINT [PK_Grupo] PRIMARY KEY CLUSTERED ([Id_Grupo] ASC) WITH (FILLFACTOR = 50)
);

