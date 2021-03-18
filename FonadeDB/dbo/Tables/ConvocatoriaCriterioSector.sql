CREATE TABLE [dbo].[ConvocatoriaCriterioSector] (
    [CodCriterio] INT NOT NULL,
    [CodSector]   INT NOT NULL,
    CONSTRAINT [PK_ConvocatoriaCriterioSector] PRIMARY KEY CLUSTERED ([CodCriterio] ASC, [CodSector] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ConvocatoriaCriterioSector_ConvocatoriaCriterio] FOREIGN KEY ([CodCriterio]) REFERENCES [dbo].[ConvocatoriaCriterio] ([Id_Criterio])
);

