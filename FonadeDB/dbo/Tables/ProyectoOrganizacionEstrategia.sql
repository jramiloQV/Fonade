CREATE TABLE [dbo].[ProyectoOrganizacionEstrategia] (
    [CodProyecto]     INT  NOT NULL,
    [AnalisisDOFA]    TEXT NULL,
    [OrganismosApoyo] TEXT NULL,
    CONSTRAINT [FK_ProyectoOrganizacionEstrategia_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

