CREATE TABLE [dbo].[EvaluacionSeguimiento] (
    [Id_EvaluacionSeguimiento]                              INT      IDENTITY (1, 1) NOT NULL,
    [FechaActualizacion]                                    DATETIME NOT NULL,
    [CodProyecto]                                           INT      NOT NULL,
    [CodContacto]                                           INT      NOT NULL,
    [LecturaPlanNegocio]                                    BIT      NULL,
    [SolicitudInformacionEmprendedor]                       BIT      NULL,
    [Antecedentes]                                          BIT      NULL,
    [DefinicionObjetivos]                                   BIT      NULL,
    [EquipoTrabajo]                                         BIT      NULL,
    [JustificacionProyecto]                                 BIT      NULL,
    [ResumenEjecutivo]                                      BIT      NULL,
    [CaracterizacionProducto]                               BIT      NULL,
    [EstrategiasGarantiasComercializacion]                  BIT      NULL,
    [IdentificacionMercadoObjetivo]                         BIT      NULL,
    [IdentificacionEvaluacionCanales]                       BIT      NULL,
    [ProyeccionVentas]                                      BIT      NULL,
    [CaracterizacionTecnicaProductoServicio]                BIT      NULL,
    [DefinicionProcesoProduccionImplementarIndicesTecnicos] BIT      NULL,
    [IdentificacionValoracionRequerimientosEquipamiento]    BIT      NULL,
    [ProgramaProduccion]                                    BIT      NULL,
    [AnalisisTramitesRequisitosLegales]                     BIT      NULL,
    [CompromisosInstitucionalesPrivadosPublicos]            BIT      NULL,
    [OrganizacionEmpresarialPropuesta]                      BIT      NULL,
    [CuantificacionInversionRequerida]                      BIT      NULL,
    [PerspectivasRentabilidad]                              BIT      NULL,
    [EstadosFinancieros]                                    BIT      NULL,
    [PresupuestosCostosProduccion]                          BIT      NULL,
    [PresupuestoIngresosOperacion]                          BIT      NULL,
    [ContemplaManejoAmbiental]                              BIT      NULL,
    [ModeloFinanciera]                                      BIT      NULL,
    [IndicesRentabilidad]                                   BIT      NULL,
    [Viabilidad]                                            BIT      NULL,
    [IndicadoresGestion]                                    BIT      NULL,
    [PlanOperativo]                                         BIT      NULL,
    [InformeEvaluacion]                                     BIT      NULL,
    [CodConvocatoria]                                       INT      NULL,
    CONSTRAINT [PK_EvaluacionSeguimiento] PRIMARY KEY CLUSTERED ([Id_EvaluacionSeguimiento] ASC) WITH (FILLFACTOR = 50)
);

