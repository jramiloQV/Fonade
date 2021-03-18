CREATE TABLE [dbo].[Texto] (
    [Id_Texto] INT           IDENTITY (1, 1) NOT NULL,
    [NomTexto] VARCHAR (100) NOT NULL,
    [Texto]    TEXT          NOT NULL,
    CONSTRAINT [PK_Texto] PRIMARY KEY CLUSTERED ([Id_Texto] ASC) WITH (FILLFACTOR = 50)
);

