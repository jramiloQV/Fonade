--USE Fonade
--GO

CREATE PROCEDURE spCrearEvaluacionesDeRiesgo
(
	@Riesgo VARCHAR(500),
	@Mitigacion VARCHAR(500)
)
AS
INSERT INTO [EvaluacionRiesgo] (Riesgo,Mitigacion,CodConvocatoria,CodProyecto)
VALUES(@Riesgo, @Mitigacion,10,9)