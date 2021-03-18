CREATE TABLE [dbo].[ProyectoEstrategiaActividades] (
    [IdActividad]        INT             IDENTITY (1, 1) NOT NULL,
    [IdProyecto]         INT             NOT NULL,
    [IdTipoEstrategia]   INT             NOT NULL,
    [Actividad]          VARCHAR (100)   NOT NULL,
    [RecursosRequeridos] VARCHAR (250)   NOT NULL,
    [MesEjecucion]       VARCHAR (250)   NOT NULL,
    [Costo]              DECIMAL (12, 2) NOT NULL,
    [Responsable]        VARCHAR (100)   NOT NULL,
    CONSTRAINT [PK_ProyectoEstrategiaActividades] PRIMARY KEY CLUSTERED ([IdActividad] ASC),
    CONSTRAINT [FK_ProyectoEstrategiaActividades_ProyectoFuturoNegocio] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto]),
    CONSTRAINT [FK_ProyectoEstrategiaActividades_TipoEstrategia] FOREIGN KEY ([IdTipoEstrategia]) REFERENCES [dbo].[TipoEstrategia] ([IdTipoEstrategia])
);

