CREATE TABLE [dbo].[ProyectoOportunidadMercadoCompetidores] (
    [IdCompetidor]          INT           IDENTITY (1, 1) NOT NULL,
    [IdProyecto]            INT           NOT NULL,
    [Nombre]                VARCHAR (100) NOT NULL,
    [Localizacion]          VARCHAR (250) NOT NULL,
    [ProductosServicios]    VARCHAR (250) NOT NULL,
    [Precios]               VARCHAR (250) NOT NULL,
    [LogisticaDistribucion] VARCHAR (250) NOT NULL,
    [OtroCual]              VARCHAR (250) NULL,
    CONSTRAINT [PK_ProyectoOportunidadMercadoCompetidores] PRIMARY KEY CLUSTERED ([IdCompetidor] ASC),
    CONSTRAINT [FK_ProyectoOportunidadMercadoCompetidores_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

