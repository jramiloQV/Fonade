﻿CREATE TABLE [dbo].[AvanceProduccionPOAnexos] (
    [CodProducto]         INT           NOT NULL,
    [NomDocumento]        VARCHAR (80)  NOT NULL,
    [CodContacto]         INT           NULL,
    [URL]                 VARCHAR (250) NULL,
    [Mes]                 TINYINT       NULL,
    [CodDocumentoFormato] INT           NULL,
    [Fecha]               DATETIME      NULL,
    [Borrado]             BIT           CONSTRAINT [DF_AvanceProduccionPOAnexos_Borrado] DEFAULT (0) NULL,
    [Comentario]          VARCHAR (255) NULL,
    [CodTipoInterventor]  INT           NULL
);

