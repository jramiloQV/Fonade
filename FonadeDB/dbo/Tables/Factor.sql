CREATE TABLE [dbo].[Factor] (
    [Id_Factor] TINYINT      IDENTITY (1, 1) NOT NULL,
    [NomFactor] VARCHAR (30) NULL,
    CONSTRAINT [PK_Factor] PRIMARY KEY CLUSTERED ([Id_Factor] ASC) WITH (FILLFACTOR = 50)
);

