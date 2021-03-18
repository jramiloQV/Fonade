CREATE TABLE [dbo].[InterventorAnexos] (
    [CodContacto]         INT           NOT NULL,
    [NomDocumento]        VARCHAR (80)  NOT NULL,
    [URL]                 VARCHAR (250) NULL,
    [CodDocumentoFormato] INT           NULL,
    [Fecha]               DATETIME      NULL,
    [Borrado]             BIT           CONSTRAINT [DF_InterventorAnexos_Borrado] DEFAULT (0) NULL,
    [Comentario]          VARCHAR (255) NULL
);

