


CREATE PROCEDURE [dbo].[spListaEvaluacionesEspecificas]
AS
SELECT Id_Riesgo, CodProyecto, CodConvocatoria, Riesgo, Mitigacion
FROM dbo.EvaluacionRiesgo
WHERE CodProyecto = 34271 AND CodConvocatoria = 80