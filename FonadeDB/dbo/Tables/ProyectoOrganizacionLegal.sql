CREATE TABLE [dbo].[ProyectoOrganizacionLegal] (
    [CodProyecto]     INT  NOT NULL,
    [AspectosLegales] TEXT NULL,
    CONSTRAINT [FK_ProyectoOrganizacionLegal_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

