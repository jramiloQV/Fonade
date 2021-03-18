CREATE TABLE [dbo].[Periodo] (
    [Id_Periodo] TINYINT      IDENTITY (1, 1) NOT NULL,
    [NomPeriodo] VARCHAR (20) NULL,
    CONSTRAINT [PK_Periodo] PRIMARY KEY CLUSTERED ([Id_Periodo] ASC) WITH (FILLFACTOR = 50)
);

