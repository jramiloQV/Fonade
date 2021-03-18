CREATE TABLE [dbo].[ProyectoOportunidadMercado] (
    [IdOportunidadMercado] INT           IDENTITY (1, 1) NOT NULL,
    [IdProyecto]           INT           NOT NULL,
    [TendenciaCrecimiento] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProyectoOportunidadMercado] PRIMARY KEY CLUSTERED ([IdOportunidadMercado] ASC),
    CONSTRAINT [FK_ProyectoOportunidadMercado_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

