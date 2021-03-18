

-- Author:		Alberto Palencia Benedetti
-- Create date:  13/03/2014
-- Description:	 Se crea para obtener lA HOJA DE AVANCE DEL EVALUADOR, COORDINADOR ETC

-- ejemplo
/*
 
 MD_HojaAvanceEvaluacion 7510
 
 */

CREATE PROCEDURE [dbo].[MD_HojaAvanceEvaluacionV2]
@IdContacto int,
@IdOpcion int
AS 
BEGIN
	if @IdOpcion=0
		begin
			SELECT        ES.IdEvaluacionSeguimiento, ES.IdProyecto, P.NomProyecto, C.Nombres + ' ' + C.Apellidos AS Evaluador, ES.IdConvocatoria, ES.IdContacto, ES.FechaActualizacion, 
						ES.LecturaPlan, ES.SolicitudInformacion, ES.IdentificacionMercado, ES.NecesidadClientes, ES.FuerzaMercado, ES.TendenciasMercado, ES.Competencia, 
						ES.PropuestaValor, ES.ValidacionMercado, ES.Antecedentes, ES.CondicionesComercializacion, ES.Normatividad, ES.OperacionNegocio, ES.EquipoTrabajo, 
						ES.EstrategiasComercializacion, ES.PeriodoImproductivo, ES.Sostenibilidad, ES.Riesgos, ES.PlanOperativo, ES.IndicadoresSeguimiento, ES.Modelo, 
						ES.IndiceRentabilidad, ES.Viabilidad, ES.IndicadoresGestion, ES.PlanOperativo2, ES.InformeEvaluacion, E.CodCoordinador
			FROM            EvaluacionSeguimientoV2 AS ES INNER JOIN
						Proyecto AS P ON ES.IdProyecto = P.Id_Proyecto INNER JOIN
						Evaluador AS E ON ES.IdContacto = E.CodContacto INNER JOIN
						Contacto AS C ON ES.IdContacto = C.Id_Contacto
			WHERE        (P.CodEstado = 4) AND (E.CodCoordinador = @IdContacto)
		end
	else if @IdOpcion=1
		begin 
			SELECT        ES.IdEvaluacionSeguimiento, ES.IdProyecto, P.NomProyecto, C.Nombres + ' ' + C.Apellidos AS Evaluador, ES.IdConvocatoria, ES.IdContacto, ES.FechaActualizacion, 
				ES.LecturaPlan, ES.SolicitudInformacion, ES.IdentificacionMercado, ES.NecesidadClientes, ES.FuerzaMercado, ES.TendenciasMercado, ES.Competencia, 
				ES.PropuestaValor, ES.ValidacionMercado, ES.Antecedentes, ES.CondicionesComercializacion, ES.Normatividad, ES.OperacionNegocio, ES.EquipoTrabajo, 
				ES.EstrategiasComercializacion, ES.PeriodoImproductivo, ES.Sostenibilidad, ES.Riesgos, ES.PlanOperativo, ES.IndicadoresSeguimiento, ES.Modelo, 
				ES.IndiceRentabilidad, ES.Viabilidad, ES.IndicadoresGestion, ES.PlanOperativo2, ES.InformeEvaluacion
			FROM            EvaluacionSeguimientoV2 AS ES INNER JOIN
				Proyecto AS P ON ES.IdProyecto = P.Id_Proyecto INNER JOIN
				Contacto AS C ON ES.IdContacto = C.Id_Contacto
			WHERE        (P.CodEstado = 4) AND (ES.IdContacto = @IdContacto)
		end
END