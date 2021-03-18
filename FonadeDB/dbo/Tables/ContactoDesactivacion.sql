CREATE TABLE [dbo].[ContactoDesactivacion] (
    [Id_ContactoDesactivacion] INT           IDENTITY (1, 1) NOT NULL,
    [CodContacto]              INT           NOT NULL,
    [FechaInicio]              SMALLDATETIME NOT NULL,
    [FechaFin]                 SMALLDATETIME NULL,
    [Comentario]               VARCHAR (255) NOT NULL,
    CONSTRAINT [ContactoDesactivacion_PK] PRIMARY KEY CLUSTERED ([Id_ContactoDesactivacion] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ContactoDesactivacion_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto])
);

