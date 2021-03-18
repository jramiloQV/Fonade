CREATE TABLE [dbo].[ProyectoOperacionInfraestructura] (
    [CodProyecto]        INT  NOT NULL,
    [ParametrosTecnicos] TEXT NULL,
    CONSTRAINT [FK_ProyectoOperacionInfraestructura_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

