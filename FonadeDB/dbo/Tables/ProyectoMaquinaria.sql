CREATE TABLE [dbo].[ProyectoMaquinaria] (
    [Id_Equipo]     INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto]   INT           NOT NULL,
    [NomEquipo]     VARCHAR (255) NOT NULL,
    [ValorUnitario] MONEY         NOT NULL,
    [Cantidad]      NUMERIC (18)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id_Equipo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoMaquinaria_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

