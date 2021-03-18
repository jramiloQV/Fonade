CREATE TABLE [dbo].[AsignacionActa] (
    [Id_Acta]         INT            IDENTITY (1, 1) NOT NULL,
    [NomActa]         VARCHAR (250)  NULL,
    [NumActa]         VARCHAR (20)   NULL,
    [FechaActa]       SMALLDATETIME  NULL,
    [Observaciones]   VARCHAR (1500) NULL,
    [CodConvocatoria] INT            NULL,
    [Publicado]       BIT            NULL,
    CONSTRAINT [PK_AsignacionActa] PRIMARY KEY CLUSTERED ([Id_Acta] ASC) WITH (FILLFACTOR = 50)
);

