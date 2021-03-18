CREATE TABLE [dbo].[PagoSucursal] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [CodPagoBanco]    INT           NOT NULL,
    [Id_PagoSucursal] INT           NULL,
    [NomPagoSucursal] VARCHAR (255) NULL,
    [CodPagoSucursal] VARCHAR (20)  NULL,
    CONSTRAINT [PK_PagoSucursal] PRIMARY KEY CLUSTERED ([Id] ASC)
);

