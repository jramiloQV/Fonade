CREATE TABLE [dbo].[Nivel] (
    [Id_Nivel] TINYINT      IDENTITY (1, 1) NOT NULL,
    [NomNivel] VARCHAR (80) NOT NULL,
    [Orden]    TINYINT      NOT NULL,
    CONSTRAINT [PK_Nivel] PRIMARY KEY CLUSTERED ([Id_Nivel] ASC) WITH (FILLFACTOR = 50)
);

