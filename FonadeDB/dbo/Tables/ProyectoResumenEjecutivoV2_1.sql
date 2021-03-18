CREATE TABLE [dbo].[ProyectoResumenEjecutivoV2] (
    [IdResumenEjecutivo]       INT           IDENTITY (1, 1) NOT NULL,
    [IdProyecto]               INT           NOT NULL,
    [ConceptoNegocio]          VARCHAR (MAX) NOT NULL,
    [IndicadorEmpleos]         VARCHAR (100) NOT NULL,
    [IndicadorVentas]          VARCHAR (100) NOT NULL,
    [IndicadorMercadeo]        VARCHAR (100) NOT NULL,
    [IndicadorContraPartido]   VARCHAR (100) NOT NULL,
    [IndicadorEmpleosDirectos] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ProyectoResumenEjecutivoV2] PRIMARY KEY CLUSTERED ([IdResumenEjecutivo] ASC),
    CONSTRAINT [FK_ProyectoResumenEjecutivoV2_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

