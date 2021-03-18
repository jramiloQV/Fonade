CREATE TABLE [dbo].[PagoTipoRetencion] (
    [Id_PagoTipoRetencion] INT           IDENTITY (1, 1) NOT NULL,
    [NomPagoTipoRetencion] VARCHAR (255) NULL,
    [Sigla]                VARCHAR (2)   NULL,
    CONSTRAINT [PK_PagoTipoRetencion] PRIMARY KEY CLUSTERED ([Id_PagoTipoRetencion] ASC) WITH (FILLFACTOR = 50)
);

