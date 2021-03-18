CREATE TABLE [dbo].[ProyectoFuturoNegocio] (
    [IdFuturoNegocio]                 INT           IDENTITY (1, 1) NOT NULL,
    [IdProyecto]                      INT           NOT NULL,
    [EstrategiaPromocion]             VARCHAR (100) NOT NULL,
    [EstrategiaPromocionProposito]    VARCHAR (100) NOT NULL,
    [EstrategiaComunicacion]          VARCHAR (100) NOT NULL,
    [EstrategiaComunicacionProposito] VARCHAR (100) NOT NULL,
    [EstrategiaDistribucion]          VARCHAR (100) NOT NULL,
    [EstrategiaDistribucionProposito] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ProyectoFuturoNegocio] PRIMARY KEY CLUSTERED ([IdFuturoNegocio] ASC),
    CONSTRAINT [FK_ProyectoFuturoNegocio_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

