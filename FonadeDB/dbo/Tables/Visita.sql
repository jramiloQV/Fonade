CREATE TABLE [dbo].[Visita] (
    [Id_Visita]      INT           IDENTITY (1, 1) NOT NULL,
    [Id_Interventor] INT           NOT NULL,
    [Id_Empresa]     INT           NOT NULL,
    [FechaInicio]    SMALLDATETIME NOT NULL,
    [FechaFin]       SMALLDATETIME NOT NULL,
    [Estado]         CHAR (24)     NOT NULL,
    [Objeto]         VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Visita] PRIMARY KEY CLUSTERED ([Id_Visita] ASC) WITH (FILLFACTOR = 50)
);

