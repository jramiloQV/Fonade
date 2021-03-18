CREATE TABLE [dbo].[InterventorInformeFinalAnexos] (
    [Id_InterventorInformeFinalAnexos] INT           IDENTITY (1, 1) NOT NULL,
    [NomInterventorInformeFinalAnexos] VARCHAR (255) NULL,
    [RutaArchivo]                      VARCHAR (255) NULL,
    [CodInformeFinal]                  INT           NULL,
    [Borrado]                          BIT           NULL,
    [CodDocumentoFormato]              INT           NULL,
    CONSTRAINT [PK_InterventorInformeFinalAnexos] PRIMARY KEY CLUSTERED ([Id_InterventorInformeFinalAnexos] ASC) WITH (FILLFACTOR = 50)
);

