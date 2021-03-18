CREATE TABLE [dbo].[ContactoArchivosAnexos] (
    [Id_ContactoArchivosAnexos] INT           IDENTITY (1, 1) NOT NULL,
    [CodContacto]               INT           NULL,
    [CodContactoEstudio]        INT           NULL,
    [ruta]                      VARCHAR (250) NULL,
    [NombreArchivo]             VARCHAR (250) NULL,
    [TipoArchivo]               VARCHAR (50)  NULL,
    [CodProyecto]               INT           NULL,
    CONSTRAINT [PK_ContactoArchivosAnexos] PRIMARY KEY CLUSTERED ([Id_ContactoArchivosAnexos] ASC) WITH (FILLFACTOR = 50)
);

