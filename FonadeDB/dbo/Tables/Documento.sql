CREATE TABLE [dbo].[Documento] (
    [Id_Documento]        INT           IDENTITY (1, 1) NOT NULL,
    [NomDocumento]        VARCHAR (256) NOT NULL,
    [URL]                 VARCHAR (256) NULL,
    [Fecha]               DATETIME      NOT NULL,
    [CodProyecto]         INT           NOT NULL,
    [CodDocumentoFormato] INT           NOT NULL,
    [CodContacto]         INT           NOT NULL,
    [Comentario]          VARCHAR (512) NULL,
    [Borrado]             BIT           CONSTRAINT [DF_Documento_Borrado] DEFAULT ((0)) NOT NULL,
    [CodTab]              SMALLINT      NULL,
    [CodEstado]           TINYINT       NULL,
    CONSTRAINT [PK_Documento] PRIMARY KEY CLUSTERED ([Id_Documento] ASC) WITH (FILLFACTOR = 70),
    CONSTRAINT [FK_Documento_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_Documento_DocumentoFormato] FOREIGN KEY ([CodDocumentoFormato]) REFERENCES [dbo].[DocumentoFormato] ([Id_DocumentoFormato]),
    CONSTRAINT [FK_Documento_Estado] FOREIGN KEY ([CodEstado]) REFERENCES [dbo].[Estado] ([Id_Estado]),
    CONSTRAINT [FK_Documento_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto]),
    CONSTRAINT [FK_Documento_Tab] FOREIGN KEY ([CodTab]) REFERENCES [dbo].[Tab] ([Id_Tab])
);



