CREATE TABLE [dbo].[Sector] (
    [Id_Sector] INT           IDENTITY (1, 1) NOT NULL,
    [Codigo]    CHAR (1)      NOT NULL,
    [NomSector] VARCHAR (150) NOT NULL,
    [Icono]     VARCHAR (60)  NULL,
    CONSTRAINT [PK_Sector] PRIMARY KEY CLUSTERED ([Id_Sector] ASC) WITH (FILLFACTOR = 50)
);

