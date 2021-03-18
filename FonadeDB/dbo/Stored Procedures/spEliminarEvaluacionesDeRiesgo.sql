CREATE PROCEDURE spEliminarEvaluacionesDeRiesgo
(
	@Id_Riesgo INT
)
AS
DELETE FROM dbo.EvaluacionRiesgo
WHERE Id_Riesgo = @Id_Riesgo