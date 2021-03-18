CREATE TABLE [dbo].[Fase] (
    [Id_Fase]  TINYINT      IDENTITY (1, 1) NOT NULL,
    [CodNivel] TINYINT      NOT NULL,
    [NomFase]  VARCHAR (80) NOT NULL,
    [Orden]    TINYINT      NOT NULL,
    CONSTRAINT [PK_Fase] PRIMARY KEY CLUSTERED ([Id_Fase] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_Fase_Nivel] FOREIGN KEY ([CodNivel]) REFERENCES [dbo].[Nivel] ([Id_Nivel])
);

