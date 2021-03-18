CREATE TABLE [dbo].[ConvocatoriaCriterioCiudad] (
    [CodCriterio]     INT NOT NULL,
    [CodDepartamento] INT NOT NULL,
    [CodCiudad]       INT NOT NULL,
    CONSTRAINT [PK_ConvocatoriaCriterioCiudad] PRIMARY KEY CLUSTERED ([CodCriterio] ASC, [CodDepartamento] ASC, [CodCiudad] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ConvocatoriaCriterioCiudad_ConvocatoriaCriterio] FOREIGN KEY ([CodCriterio]) REFERENCES [dbo].[ConvocatoriaCriterio] ([Id_Criterio])
);

