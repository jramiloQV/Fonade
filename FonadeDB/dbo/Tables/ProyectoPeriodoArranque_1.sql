CREATE TABLE [dbo].[ProyectoPeriodoArranque] (
    [IdPeriodoArranque]   INT           IDENTITY (1, 1) NOT NULL,
    [IdProyecto]          INT           NOT NULL,
    [PeriodoArranque]     VARCHAR (MAX) NOT NULL,
    [PeriodoImproductivo] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProyectoPeriodoArranque] PRIMARY KEY CLUSTERED ([IdPeriodoArranque] ASC),
    CONSTRAINT [FK_ProyectoPeriodoArranque_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

