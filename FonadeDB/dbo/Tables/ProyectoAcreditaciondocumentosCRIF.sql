CREATE TABLE [dbo].[ProyectoAcreditaciondocumentosCRIF] (
    [Id_ProyectoAcreditaciondocumentosCRIF] INT          IDENTITY (1, 1) NOT NULL,
    [CodProyecto]                           INT          NOT NULL,
    [CodConvocatoria]                       INT          NOT NULL,
    [Crif]                                  VARCHAR (50) NOT NULL,
    [Fecha]                                 DATETIME     NOT NULL,
    CONSTRAINT [PK_ProyectoAcreditaciondocumentosCRIF] PRIMARY KEY CLUSTERED ([Id_ProyectoAcreditaciondocumentosCRIF] ASC) WITH (FILLFACTOR = 50)
);

