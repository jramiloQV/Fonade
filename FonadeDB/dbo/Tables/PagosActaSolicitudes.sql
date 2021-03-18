CREATE TABLE [dbo].[PagosActaSolicitudes] (
    [Id_Acta]                INT           IDENTITY (1, 1) NOT NULL,
    [Fecha]                  DATETIME      NULL,
    [NumSolicitudes]         INT           NULL,
    [Datos]                  TEXT          NULL,
    [Firma]                  TEXT          NULL,
    [CodContacto]            INT           NULL,
    [CodRechazoFirmaDigital] INT           NULL,
    [Tipo]                   VARCHAR (50)  NULL,
    [DatosFirma]             TEXT          NULL,
    [DescargadoFA]           BIT           DEFAULT (0) NULL,
    [ArchivoPagosFA]         VARCHAR (255) NULL,
    [ArchivoTercerosFA]      VARCHAR (255) NULL,
    [CodActaFonade]          INT           NULL,
    [CodContactoFiduciaria]  INT           NULL,
    CONSTRAINT [PK_PagosActaSolicitudes] PRIMARY KEY CLUSTERED ([Id_Acta] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_PagosActaSolicitudes_PagosRechazosFirmaDigital] FOREIGN KEY ([CodRechazoFirmaDigital]) REFERENCES [dbo].[PagosRechazosFirmaDigital] ([Id_Rechazo]) NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[PagosActaSolicitudes] NOCHECK CONSTRAINT [FK_PagosActaSolicitudes_PagosRechazosFirmaDigital];

