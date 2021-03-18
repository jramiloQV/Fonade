
CREATE PROCEDURE [dbo].[MD_MostrarProyectosaPriorizar]

	@CodigoEstadoConst int
AS

BEGIN

	/*SELECT CP.CodProyecto as codigoproyecto
	, P.NomProyecto as nombreproyecto
	, MAX(CP.CodConvocatoria) AS codigoconvocatoria 
	, C.NomConvocatoria as nombreconvocatoria
	, '$' + cast((SM.SalarioMinimo * isnull(EV.ValorRecomendado,0)) as varchar(50)) valorrecomendado
	, year(C.FechaInicio) as anio
	, PT.Total as total 

	FROM dbo.ConvocatoriaProyecto CP
	inner join Convocatoria C on C.Id_Convocatoria= CP.CodConvocatoria
	inner join Proyecto P on P.Id_Proyecto= CP.CodProyecto
	inner join EvaluacionObservacion EV on EV.CodProyecto=CP.CodProyecto
	and EV.CodConvocatoria=C.Id_Convocatoria
	inner join PuntajeTotalPriorizacion PT on PT.CodConvocatoria=CP.CodConvocatoria
	and PT.CodProyecto=CP.CodProyecto 
	inner join SalariosMinimos SM on SM.AñoSalario = year(C.FechaInicio)
	WHERE CP.Viable = 1 
	and P.CodEstado=@CodigoEstadoConst
	GROUP BY CP.CodProyecto, P.NomProyecto, C.NomConvocatoria, EV.ValorRecomendado, C.FechaInicio, PT.Total, SM.SalarioMinimo
	order by CP.CodProyecto */

select id_proyecto codigoproyecto, nomproyecto nombreproyecto, cp.codconvocatoria codigoconvocatoria, nomconvocatoria nombreconvocatoria, 
'$' + FORMAT(  CAST((isnull(valorrecomendado,0) * (Select Round(SalarioMinimo,0) from SalariosMinimos where AñoSalario = year(fechainicio))) as money),'#,0.00') valorrecomendado,
year(fechainicio) anio, 
isnull(valorrecomendado,0) as CantSalarios, total
from proyecto,(SELECT CodProyecto, MAX(CodConvocatoria) AS CodConvocatoria FROM dbo.ConvocatoriaProyecto
WHERE (Viable = 1) GROUP BY CodProyecto) AS CP,convocatoria, evaluacionobservacion e, puntajetotalpriorizacion p
where id_proyecto=cp.codproyecto and id_convocatoria=cp.codconvocatoria and id_proyecto=e.codproyecto and 
id_convocatoria=e.codconvocatoria and id_proyecto=p.codproyecto and id_convocatoria=p.codconvocatoria 
and codestado= @CodigoEstadoConst order by total desc, cp.codconvocatoria, id_proyecto
END