CREATE TABLE [dbo].[LegalizacionActa] (
    [Id_Acta]         INT            IDENTITY (1, 1) NOT NULL,
    [NomActa]         VARCHAR (100)  NULL,
    [NumActa]         VARCHAR (30)   NULL,
    [FechaActa]       SMALLDATETIME  NULL,
    [Observaciones]   VARCHAR (1500) NULL,
    [Publicado]       BIT            NULL,
    [CodConvocatoria] INT            NULL,
    CONSTRAINT [PK_LegalizacionActa] PRIMARY KEY CLUSTERED ([Id_Acta] ASC) WITH (FILLFACTOR = 50)
);

