CREATE TABLE [dbo].[FirmasAutorizadas] (
    [Id_FirmasAutorizadas] INT      IDENTITY (1, 1) NOT NULL,
    [CodContacto]          INT      NOT NULL,
    [Autorizado]           BIT      NULL,
    [FechaInicio]          DATETIME NULL,
    [FechaFin]             DATETIME NULL,
    CONSTRAINT [PK_FirmasAutorizadas] PRIMARY KEY CLUSTERED ([Id_FirmasAutorizadas] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_FirmasAutorizadas_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto])
);


GO
ALTER TABLE [dbo].[FirmasAutorizadas] NOCHECK CONSTRAINT [FK_FirmasAutorizadas_Contacto];

