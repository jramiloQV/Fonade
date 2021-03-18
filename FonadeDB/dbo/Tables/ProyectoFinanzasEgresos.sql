CREATE TABLE [dbo].[ProyectoFinanzasEgresos] (
    [CodProyecto]            INT        NOT NULL,
    [ActualizacionMonetaria] FLOAT (53) NOT NULL,
    CONSTRAINT [FK_ProyectoFinanzasEgresos_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

