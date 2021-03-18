CREATE TABLE [dbo].[PagoActividadarchivo] (
    [Id_PagoActividadArchivo] INT           IDENTITY (1, 1) NOT NULL,
    [NomPagoActividadarchivo] VARCHAR (100) NULL,
    [CodPagoActividad]        INT           NULL,
    [RutaArchivo]             VARCHAR (255) NULL,
    [Icono]                   VARCHAR (255) NULL,
    [URL]                     VARCHAR (255) NULL,
    [Fecha]                   DATETIME      CONSTRAINT [DF_PagoActividadarchivo_Fecha] DEFAULT (getdate()) NULL,
    [CodTipoArchivo]          INT           NULL,
    [CodDocumentoFormato]     INT           NULL,
    CONSTRAINT [PK_PagoActividadarchivo] PRIMARY KEY CLUSTERED ([Id_PagoActividadArchivo] ASC) WITH (FILLFACTOR = 50)
);

