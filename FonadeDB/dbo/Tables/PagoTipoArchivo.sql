CREATE TABLE [dbo].[PagoTipoArchivo] (
    [Id_PagoTipoArchivo] INT           IDENTITY (1, 1) NOT NULL,
    [NomPagoTipoArchivo] VARCHAR (100) NULL,
    CONSTRAINT [PK_PagoTipoArchivo] PRIMARY KEY CLUSTERED ([Id_PagoTipoArchivo] ASC) WITH (FILLFACTOR = 50)
);

