
CREATE PROCEDURE [dbo].[MD_ImprimirActaAsignacion]

	@IdActa int

AS

BEGIN

	SELECT Proyecto.Id_Proyecto
	, Proyecto.NomProyecto
	, case when Evaluacionobservacion.valorrecomendado is null then 0 else Evaluacionobservacion.valorrecomendado end  
	AS salarios
	, case when AsignacionActaProyecto.Asignado = 1 then 'SI' else 'NO' end as Asignado
	, cast((SM.SalarioMinimo * isnull(EvaluacionObservacion.ValorRecomendado,0)) as decimal(30,2)) valorrecomendado
	, YEAR(Convocatoria.FechaFin) AS Anno
	FROM AsignacionActaProyecto
	INNER JOIN AsignacionActa
	ON AsignacionActaProyecto.CodActa = AsignacionActa.Id_Acta
	INNER JOIN Proyecto
	ON AsignacionActaProyecto.CodProyecto = Proyecto.Id_Proyecto
	INNER JOIN EvaluacionObservacion
	ON Proyecto.Id_Proyecto = EvaluacionObservacion.CodProyecto
	AND AsignacionActa.CodConvocatoria = EvaluacionObservacion.CodConvocatoria
	INNER JOIN Convocatoria
	ON EvaluacionObservacion.CodConvocatoria = Convocatoria.Id_Convocatoria
	inner join SalariosMinimos SM on SM.AñoSalario = YEAR(Convocatoria.FechaFin)
	WHERE (AsignacionActa.Id_Acta = @IdActa)
		
END