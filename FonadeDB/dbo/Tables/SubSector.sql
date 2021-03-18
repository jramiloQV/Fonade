CREATE TABLE [dbo].[SubSector] (
    [Id_SubSector] INT           IDENTITY (1, 1) NOT NULL,
    [Codigo]       CHAR (5)      NOT NULL,
    [NomSubSector] VARCHAR (150) NOT NULL,
    [CodSector]    INT           NOT NULL,
    [Rango]        TINYINT       NULL,
    CONSTRAINT [PK_SubSector] PRIMARY KEY CLUSTERED ([Id_SubSector] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_SubSector_Sector] FOREIGN KEY ([CodSector]) REFERENCES [dbo].[Sector] ([Id_Sector])
);

