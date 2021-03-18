-- =============================================
-- Author:		Carlos Arias
-- Create date: 04-04-2017
-- Description:	Realiza la consulta de la información presentada
-- en el reporte de puntaje 
-- =============================================
CREATE PROCEDURE [dbo].[REP_ConsultarReportePuntaje](@pCodigoConvocatoria AS INT) 
AS
BEGIN
	SELECT	Proyecto.NomProyecto,
		Proyecto.Id_Proyecto,
		Ciudad.NomCiudad,
		Departamento.NomDepartamento,
		Institucion.NomInstitucion,
		Institucion.NomUnidad,
		ISNULL(Pfi.Recursos, 0) MontoSolicitado,
		ISNULL(Ev.Valorrecomendado,0) as MontoRecomendado, 
		CASE WHEN ISNULL(viable, 0) = 1 THEN 'SI' ELSE 'NO' END Viable,
		TablaPuntaje.*
FROM Proyecto
INNER JOIN Ciudad ON Ciudad.Id_Ciudad = Proyecto.CodCiudad
INNER JOIN Departamento ON Departamento.Id_Departamento = Ciudad.CodDepartamento
INNER JOIN Institucion ON Institucion.Id_Institucion = Proyecto.CodInstitucion
INNER JOIN ConvocatoriaProyecto Cp ON cp.CodProyecto = Proyecto.Id_Proyecto AND Cp.CodConvocatoria = @pCodigoConvocatoria
LEFT JOIN ProyectoFinanzasIngresos Pfi ON Pfi.CodProyecto = Proyecto.Id_Proyecto
LEFT JOIN EvaluacionObservacion Ev ON Ev.CodProyecto = Proyecto.Id_Proyecto
AND Ev.CodConvocatoria = Cp.CodConvocatoria
LEFT JOIN (
SELECT	Pvt.codProyecto, Pvt.[71] A, Pvt.[73] B, 
		Pvt.[76] C, Pvt.[77] D, Pvt.[79] E, Pvt.[80] F, Pvt.[81] G, Pvt.[82] H, Pvt.[84] I, Pvt.[85] J, Pvt.[86] K, 
		Pvt.[89] L, Pvt.[90] M, Pvt.[91] N, Pvt.[92] O, Pvt.[93] P, Pvt.[95] Q, Pvt.[96] R, Pvt.[97] S, Pvt.[99] T, 
		Pvt.[100] U, Pvt.[101] V, Pvt.[104] W, Pvt.[105] X, Pvt.[106] Y, Pvt.[107] Z, Pvt.[108] AA, Pvt.[110] AB,
		Pvt.[111] AC, Pvt.[113] AD, Pvt.[114] AE, Pvt.[115] AF, Pvt.[116] AG, Pvt.[117] AH, Pvt.[118] AI, Pvt.[119] AJ,
		Pvt.[121] AK, Pvt.[122] AL, Pvt.[125] AM, Pvt.[126] AN, Pvt.[128] AO, Pvt.[129] AP, Pvt.[130] AQ, Pvt.[132] AR, 
		Pvt.[133] "AS", Pvt.[134] AT, Pvt.[135] AU, Pvt.[138] AV, Pvt.[139] AW, Pvt.[142] AX, Pvt.[143] AY, Pvt.[145] AZ

FROM
(
SELECT	Campo.id_Campo, Ec.codProyecto,
		CASE WHEN Ec.Puntaje IS NULL THEN 'N.A'
			 WHEN Campo.TipoCampo = 1 AND Ec.Puntaje = 0 THEN 'NO'
			 WHEN Campo.TipoCampo = 1 AND Ec.Puntaje = 1 THEN 'SI'
			 ELSE CAST(Ec.Puntaje AS NVARCHAR)
		END Puntaje
FROM  Campo
LEFT JOIN EvaluacionCampo Ec ON Ec.codCampo = Campo.id_Campo
INNER JOIN ConvocatoriaCampo Cc ON Ec.codConvocatoria = Cc.codConvocatoria
AND Ec.codCampo = Cc.codCampo
WHERE Ec.codConvocatoria = @pCodigoConvocatoria 
AND campo.IdVersionProyecto = 2 
AND campo.codCampo IS NOT NULL 
AND ValorPorDefecto > 0
) AS source
PIVOT
(
    MAX(puntaje)
    FOR id_campo IN ([71],[73],[76],[77],[79],[80],[81],[82],[84],[85],[86],[89],[90],[91],
					[92],[93],[95],[96],[97],[99],[100],[101],[104],[105],[106],[107],[108],
					[110],[111],[113],[114],[115],[116],[117],[118],[119],[121],[122],[125],
					[126],[128],[129],[130],[132],[133],[134],[135],[138],[139],[142],[143],[145])
) as pvt) TablaPuntaje
ON TablaPuntaje.codProyecto = Proyecto.Id_Proyecto
ORDER BY NomDepartamento, NomCiudad
END