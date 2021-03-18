CREATE TABLE [dbo].[BicatocaEventos] (
    [Id_Evento] INT           IDENTITY (1, 1) NOT NULL,
    [Evento]    NVARCHAR (50) NULL,
    CONSTRAINT [PK_BicatocaEventos] PRIMARY KEY CLUSTERED ([Id_Evento] ASC)
);

