CREATE TABLE [dbo].[ProyectoImpacto] (
    [CodProyecto] INT  NOT NULL,
    [Impacto]     TEXT NULL,
    CONSTRAINT [FK_ProyectoImpacto_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto]) ON UPDATE CASCADE
);

