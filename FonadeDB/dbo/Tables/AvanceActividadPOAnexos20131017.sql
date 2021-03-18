CREATE TABLE [dbo].[AvanceActividadPOAnexos20131017] (
    [CodActividad]        INT           NOT NULL,
    [NomDocumento]        VARCHAR (80)  NOT NULL,
    [CodContacto]         INT           NULL,
    [URL]                 VARCHAR (250) NULL,
    [Mes]                 TINYINT       NULL,
    [CodDocumentoFormato] INT           NULL,
    [Fecha]               DATETIME      NULL,
    [Borrado]             BIT           NULL,
    [Comentario]          VARCHAR (255) NULL,
    [CodTipoInterventor]  INT           NULL
);

