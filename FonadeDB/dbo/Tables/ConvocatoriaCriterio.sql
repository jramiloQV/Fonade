CREATE TABLE [dbo].[ConvocatoriaCriterio] (
    [Id_Criterio]     INT          IDENTITY (1, 1) NOT NULL,
    [CodConvocatoria] INT          NOT NULL,
    [NomCriterio]     VARCHAR (80) NOT NULL,
    CONSTRAINT [PK_ConvocatoriaCriterio] PRIMARY KEY CLUSTERED ([Id_Criterio] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ConvocatoriaCriterio_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria])
);

