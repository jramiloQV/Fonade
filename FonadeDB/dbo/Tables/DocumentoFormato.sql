CREATE TABLE [dbo].[DocumentoFormato] (
    [Id_DocumentoFormato] INT          IDENTITY (1, 1) NOT NULL,
    [NomDocumentoFormato] VARCHAR (80) NOT NULL,
    [Extension]           CHAR (8)     NULL,
    [Icono]               VARCHAR (64) NULL,
    CONSTRAINT [PK_DocumentoFormato] PRIMARY KEY CLUSTERED ([Id_DocumentoFormato] ASC) WITH (FILLFACTOR = 50)
);



