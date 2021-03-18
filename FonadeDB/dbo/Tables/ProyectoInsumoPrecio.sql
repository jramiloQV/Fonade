CREATE TABLE [dbo].[ProyectoInsumoPrecio] (
    [CodInsumo] INT     NOT NULL,
    [Periodo]   TINYINT NOT NULL,
    [Precio]    MONEY   NULL,
    CONSTRAINT [PK_ProyectoInsumoPrecio] PRIMARY KEY CLUSTERED ([CodInsumo] ASC, [Periodo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoInsumoPrecio_ProyectoInsumo] FOREIGN KEY ([CodInsumo]) REFERENCES [dbo].[ProyectoInsumo] ([Id_Insumo])
);

