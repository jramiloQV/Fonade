CREATE TABLE [dbo].[EvaluacionSeguimientoV2] (
    [IdEvaluacionSeguimiento]     INT      IDENTITY (1, 1) NOT NULL,
    [IdConvocatoria]              INT      NOT NULL,
    [IdProyecto]                  INT      NOT NULL,
    [IdContacto]                  INT      NOT NULL,
    [FechaActualizacion]          DATETIME CONSTRAINT [DF_EvaluacionSeguimientoV2_FechaActualizacion] DEFAULT (getdate()) NOT NULL,
    [LecturaPlan]                 BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_LecturaPlan] DEFAULT ((0)) NOT NULL,
    [SolicitudInformacion]        BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_SolicitudInformacion] DEFAULT ((0)) NOT NULL,
    [IdentificacionMercado]       BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_IdentificacionMercado] DEFAULT ((0)) NOT NULL,
    [NecesidadClientes]           BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_NecesidadClientes] DEFAULT ((0)) NOT NULL,
    [FuerzaMercado]               BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_FuerzaMercado] DEFAULT ((0)) NOT NULL,
    [TendenciasMercado]           BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_TendenciasMercado] DEFAULT ((0)) NOT NULL,
    [Competencia]                 BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_Competencia] DEFAULT ((0)) NOT NULL,
    [PropuestaValor]              BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_PropuestaValor] DEFAULT ((0)) NOT NULL,
    [ValidacionMercado]           BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_ValidacionMercado] DEFAULT ((0)) NOT NULL,
    [Antecedentes]                BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_Antecedentes] DEFAULT ((0)) NOT NULL,
    [CondicionesComercializacion] BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_CondicionesComercializacion] DEFAULT ((0)) NOT NULL,
    [Normatividad]                BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_Normatividad] DEFAULT ((0)) NOT NULL,
    [OperacionNegocio]            BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_OperacionNegocio] DEFAULT ((0)) NOT NULL,
    [EquipoTrabajo]               BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_EquipoTrabajo] DEFAULT ((0)) NOT NULL,
    [EstrategiasComercializacion] BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_EstrategiasComercializacion] DEFAULT ((0)) NOT NULL,
    [PeriodoImproductivo]         BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_PeriodoImproductivo] DEFAULT ((0)) NOT NULL,
    [Sostenibilidad]              BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_Sostenibilidad] DEFAULT ((0)) NOT NULL,
    [Riesgos]                     BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_Riesgos] DEFAULT ((0)) NOT NULL,
    [PlanOperativo]               BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_PlanOperativo] DEFAULT ((0)) NOT NULL,
    [IndicadoresSeguimiento]      BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_IndicadoresSeguimiento] DEFAULT ((0)) NOT NULL,
    [Modelo]                      BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_Modelo] DEFAULT ((0)) NOT NULL,
    [IndiceRentabilidad]          BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_IndiceRentabilidad] DEFAULT ((0)) NOT NULL,
    [Viabilidad]                  BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_Viabilidad] DEFAULT ((0)) NOT NULL,
    [IndicadoresGestion]          BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_IndicadoresGestion] DEFAULT ((0)) NOT NULL,
    [PlanOperativo2]              BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_PlanOperativo2] DEFAULT ((0)) NOT NULL,
    [InformeEvaluacion]           BIT      CONSTRAINT [DF_EvaluacionSeguimientoV2_IndicadoresGestion2] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EvaluacionSeguimientoV2] PRIMARY KEY CLUSTERED ([IdEvaluacionSeguimiento] ASC),
    CONSTRAINT [FK_EvaluacionSeguimientoV2_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);



