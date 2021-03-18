

-- Author:		Alberto Palencia Benedetti
-- Create date:  13/03/2014
-- Description:	 Se crea para obtener lA HOJA DE AVANCE DEL EVALUADOR, COORDINADOR ETC

-- ejemplo
/*
 
 MD_HojaAvanceEvaluacion 7510
 
 */

CREATE PROCEDURE [dbo].[MD_HojaAvanceEvaluacion]

 @IdContacto int,
 @IdOpcion int
AS 

BEGIN
	if @IdOpcion=0 --hoja de avance de los planes de un evaluador
		begin
			SELECT      P.NomProyecto, ES.Id_EvaluacionSeguimiento, ES.FechaActualizacion, ES.CodProyecto, ES.CodContacto, ES.LecturaPlanNegocio, 
						ES.SolicitudInformacionEmprendedor, ES.Antecedentes, ES.DefinicionObjetivos, ES.EquipoTrabajo, ES.JustificacionProyecto, ES.ResumenEjecutivo, 
						ES.CaracterizacionProducto, ES.EstrategiasGarantiasComercializacion, ES.IdentificacionMercadoObjetivo, ES.IdentificacionEvaluacionCanales, 
						ES.ProyeccionVentas, ES.CaracterizacionTecnicaProductoServicio, ES.DefinicionProcesoProduccionImplementarIndicesTecnicos, 
						ES.IdentificacionValoracionRequerimientosEquipamiento, ES.ProgramaProduccion, ES.AnalisisTramitesRequisitosLegales, 
						ES.CompromisosInstitucionalesPrivadosPublicos, ES.OrganizacionEmpresarialPropuesta, ES.CuantificacionInversionRequerida, ES.PerspectivasRentabilidad, 
						ES.EstadosFinancieros, ES.PresupuestosCostosProduccion, ES.PresupuestoIngresosOperacion, ES.ContemplaManejoAmbiental, ES.ModeloFinanciera, 
						ES.IndicesRentabilidad, ES.Viabilidad, ES.IndicadoresGestion, ES.PlanOperativo, ES.InformeEvaluacion, ES.CodConvocatoria
			FROM        EvaluacionSeguimiento AS ES INNER JOIN
						Proyecto AS P ON ES.CodProyecto = P.Id_Proyecto
			WHERE       (P.CodEstado = 4) AND (ES.CodContacto = @IdContacto)
		end
	else if @IdOpcion=1 --hoja de avance de los evaluadores de un coordinador
		begin
			SELECT      P.NomProyecto, ES.Id_EvaluacionSeguimiento, ES.FechaActualizacion, ES.CodProyecto, ES.CodContacto, ES.LecturaPlanNegocio, 
						ES.SolicitudInformacionEmprendedor, ES.Antecedentes, ES.DefinicionObjetivos, ES.EquipoTrabajo, ES.JustificacionProyecto, ES.ResumenEjecutivo, 
						ES.CaracterizacionProducto, ES.EstrategiasGarantiasComercializacion, ES.IdentificacionMercadoObjetivo, ES.IdentificacionEvaluacionCanales, 
						ES.ProyeccionVentas, ES.CaracterizacionTecnicaProductoServicio, ES.DefinicionProcesoProduccionImplementarIndicesTecnicos, 
						ES.IdentificacionValoracionRequerimientosEquipamiento, ES.ProgramaProduccion, ES.AnalisisTramitesRequisitosLegales, 
						ES.CompromisosInstitucionalesPrivadosPublicos, ES.OrganizacionEmpresarialPropuesta, ES.CuantificacionInversionRequerida, ES.PerspectivasRentabilidad, 
						ES.EstadosFinancieros, ES.PresupuestosCostosProduccion, ES.PresupuestoIngresosOperacion, ES.ContemplaManejoAmbiental, ES.ModeloFinanciera, 
						ES.IndicesRentabilidad, ES.Viabilidad, ES.IndicadoresGestion, ES.PlanOperativo, ES.InformeEvaluacion, ES.CodConvocatoria
			FROM        EvaluacionSeguimiento AS ES INNER JOIN
						Proyecto AS P ON ES.CodProyecto = P.Id_Proyecto INNER JOIN
						Evaluador AS E ON ES.CodContacto = E.CodContacto
			WHERE       (P.CodEstado = 4) AND (E.CodCoordinador = @IdContacto)
		end	
END