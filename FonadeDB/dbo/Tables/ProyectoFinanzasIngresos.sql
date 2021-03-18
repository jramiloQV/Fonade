CREATE TABLE [dbo].[ProyectoFinanzasIngresos] (
    [CodProyecto] INT     NOT NULL,
    [Recursos]    TINYINT NOT NULL,
    CONSTRAINT [FK_ProyectoFinanzasIngresos_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

