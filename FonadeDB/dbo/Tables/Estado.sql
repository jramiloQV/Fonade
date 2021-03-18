CREATE TABLE [dbo].[Estado] (
    [Id_Estado] TINYINT      IDENTITY (1, 1) NOT NULL,
    [CodFase]   TINYINT      NOT NULL,
    [NomEstado] VARCHAR (80) NOT NULL,
    [Orden]     TINYINT      NOT NULL,
    CONSTRAINT [PK_Estado] PRIMARY KEY CLUSTERED ([Id_Estado] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_Estado_Fase] FOREIGN KEY ([CodFase]) REFERENCES [dbo].[Fase] ([Id_Fase])
);

