CREATE TABLE [dbo].[ProyectoProductividad] (
    [IdProductividad]     INT           IDENTITY (1, 1) NOT NULL,
    [IdProyecto]          INT           NOT NULL,
    [CapacidadProductiva] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProyectoProductividad] PRIMARY KEY CLUSTERED ([IdProductividad] ASC),
    CONSTRAINT [FK_ProyectoProductividad_ProyectoDesarrolloSolucion] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

