
CREATE PROCEDURE [dbo].[MD_VerActaAsignacionRecursos]

	@IdActa int

AS

BEGIN
	--SELECT Proyecto.Id_Proyecto, Upper(Proyecto.NomProyecto) NomProyecto, 
	--case when Evaluacionobservacion.valorrecomendado is null then 0 else Evaluacionobservacion.valorrecomendado end  
	--AS salarios,
	--case when AsignacionActaProyecto.Asignado = 1 then 'SI' else 'NO' end as Asignado
	--, YEAR(Convocatoria.FechaFin) AS Anno 
	--,ISNULL((Round(cast((SM.SalarioMinimo * isnull(EvaluacionObservacion.ValorRecomendado,0)) as decimal(30,2)), -3)),0) valorrecomendado
	--FROM AsignacionActaProyecto 
	--INNER JOIN AsignacionActa 
	--	ON AsignacionActaProyecto.CodActa = AsignacionActa.Id_Acta 
	--INNER JOIN Proyecto 
	--	ON AsignacionActaProyecto.CodProyecto = Proyecto.Id_Proyecto 
	--LEFT JOIN EvaluacionObservacion 
	--	ON Proyecto.Id_Proyecto = EvaluacionObservacion.CodProyecto 
	--	AND AsignacionActa.CodConvocatoria = EvaluacionObservacion.CodConvocatoria
	--INNER JOIN Convocatoria 
	--	ON AsignacionActa.CodConvocatoria = Convocatoria.Id_Convocatoria 
	--Left join SalariosMinimos SM on SM.AñoSalario = YEAR(Convocatoria.FechaFin)
	--WHERE (AsignacionActa.Id_Acta = @IdActa) order by 1

	SELECT Proyecto.Id_Proyecto, Upper(Proyecto.NomProyecto) NomProyecto, 
	case when Evaluacionobservacion.valorrecomendado is null then 0 else Evaluacionobservacion.valorrecomendado end  
	AS salarios,
	case when AsignacionActaProyecto.Asignado = 1 then 'SI' else 'NO' end as Asignado
	, YEAR(Convocatoria.FechaFin) AS Anno 
	,ISNULL((cast((Round(SM.SalarioMinimo, -3) * isnull(EvaluacionObservacion.ValorRecomendado,0)) as decimal(30,2))),0) valorrecomendado
	FROM AsignacionActaProyecto 
	INNER JOIN AsignacionActa 
		ON AsignacionActaProyecto.CodActa = AsignacionActa.Id_Acta 
	INNER JOIN Proyecto 
		ON AsignacionActaProyecto.CodProyecto = Proyecto.Id_Proyecto 
	LEFT JOIN EvaluacionObservacion 
		ON Proyecto.Id_Proyecto = EvaluacionObservacion.CodProyecto 
		AND AsignacionActa.CodConvocatoria = EvaluacionObservacion.CodConvocatoria
	INNER JOIN Convocatoria 
		ON AsignacionActa.CodConvocatoria = Convocatoria.Id_Convocatoria 
	Left join SalariosMinimos SM on SM.AñoSalario = YEAR(Convocatoria.FechaFin)
	WHERE (AsignacionActa.Id_Acta = @IdActa) order by 1
		
	Select Round(salarioMinimo, -3) from SalariosMinimos where AñoSalario = (
	Select year(fechafin) from convocatoria where Id_Convocatoria = (
	Select CodConvocatoria from asignacionActa where id_acta = @IdActa))
END