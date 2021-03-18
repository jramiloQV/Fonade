CREATE TABLE [dbo].[ProyectoOrganizacionEstructura] (
    [CodProyecto]              INT  NOT NULL,
    [EstructuraOrganizacional] TEXT NULL,
    CONSTRAINT [FK_ProyectoOrganizacionEstructura_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

