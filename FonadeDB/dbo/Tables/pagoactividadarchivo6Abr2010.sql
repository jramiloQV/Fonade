CREATE TABLE [dbo].[pagoactividadarchivo6Abr2010] (
    [Id_PagoActividadArchivo] INT           IDENTITY (1, 1) NOT NULL,
    [NomPagoActividadarchivo] VARCHAR (100) NULL,
    [CodPagoActividad]        INT           NULL,
    [RutaArchivo]             VARCHAR (255) NULL,
    [Icono]                   VARCHAR (255) NULL,
    [URL]                     VARCHAR (255) NULL,
    [Fecha]                   DATETIME      NULL,
    [CodTipoArchivo]          INT           NULL,
    [CodDocumentoFormato]     INT           NULL
);

