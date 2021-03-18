CREATE TABLE [dbo].[ProyectoRiesgos] (
    [IdRiesgo]         INT           IDENTITY (1, 1) NOT NULL,
    [IdProyecto]       INT           NOT NULL,
    [ActoresExternos]  VARCHAR (MAX) NOT NULL,
    [FactoresExternos] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProyectoRiesgos] PRIMARY KEY CLUSTERED ([IdRiesgo] ASC),
    CONSTRAINT [FK_ProyectoRiesgos_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

