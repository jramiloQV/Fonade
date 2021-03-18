CREATE TABLE [dbo].[PagosActaSolicitudPagos] (
    [CodPagosActaSolicitudes] INT            NOT NULL,
    [CodPagoActividad]        INT            NOT NULL,
    [Aprobado]                BIT            NULL,
    [Observaciones]           VARCHAR (2000) NULL,
    CONSTRAINT [PK_PagosActaSolicitudPagos] PRIMARY KEY CLUSTERED ([CodPagosActaSolicitudes] ASC, [CodPagoActividad] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_PagosActaSolicitudPagos_PagoActividad] FOREIGN KEY ([CodPagoActividad]) REFERENCES [dbo].[PagoActividad] ([Id_PagoActividad]) NOT FOR REPLICATION,
    CONSTRAINT [FK_PagosActaSolicitudPagos_PagosActaSolicitudes] FOREIGN KEY ([CodPagosActaSolicitudes]) REFERENCES [dbo].[PagosActaSolicitudes] ([Id_Acta]) NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[PagosActaSolicitudPagos] NOCHECK CONSTRAINT [FK_PagosActaSolicitudPagos_PagoActividad];


GO
ALTER TABLE [dbo].[PagosActaSolicitudPagos] NOCHECK CONSTRAINT [FK_PagosActaSolicitudPagos_PagosActaSolicitudes];

