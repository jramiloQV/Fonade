CREATE TABLE [dbo].[ProyectoInsumoUnidadesCompras] (
    [CodInsumo] INT          NOT NULL,
    [Unidades]  NUMERIC (18) NOT NULL,
    [Mes]       SMALLINT     NOT NULL,
    [Ano]       SMALLINT     NOT NULL,
    CONSTRAINT [FK_ProyectoInsumoUnidadesCompras_ProyectoInsumo] FOREIGN KEY ([CodInsumo]) REFERENCES [dbo].[ProyectoInsumo] ([Id_Insumo])
);

