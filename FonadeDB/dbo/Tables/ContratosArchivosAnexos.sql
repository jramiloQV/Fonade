CREATE TABLE [dbo].[ContratosArchivosAnexos] (
    [IdContratoArchivoAnexo] INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto]            INT           NULL,
    [ruta]                   VARCHAR (250) NULL,
    [NombreArchivo]          VARCHAR (250) NULL,
    CONSTRAINT [PK_ContratosArchivosAnexos] PRIMARY KEY CLUSTERED ([IdContratoArchivoAnexo] ASC)
);

