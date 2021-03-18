CREATE TABLE [dbo].[ConvocatoriaActa] (
    [Id_Acta]             INT           IDENTITY (1, 1) NOT NULL,
    [NumActa]             VARCHAR (20)  NOT NULL,
    [NomActa]             VARCHAR (80)  NOT NULL,
    [FechaActa]           DATETIME      NOT NULL,
    [Fecha]               DATETIME      NOT NULL,
    [URL]                 VARCHAR (255) NOT NULL,
    [CodConvocatoria]     INT           NOT NULL,
    [CodDocumentoFormato] INT           NOT NULL,
    [CodContacto]         INT           NOT NULL,
    [Comentario]          VARCHAR (255) NULL,
    [Borrado]             BIT           CONSTRAINT [DF_ConvocatoriaActa_Borrado] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ConvocatoriaActa] PRIMARY KEY CLUSTERED ([Id_Acta] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ConvocatoriaActa_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_ConvocatoriaActa_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_ConvocatoriaActa_DocumentoFormato] FOREIGN KEY ([CodDocumentoFormato]) REFERENCES [dbo].[DocumentoFormato] ([Id_DocumentoFormato])
);



