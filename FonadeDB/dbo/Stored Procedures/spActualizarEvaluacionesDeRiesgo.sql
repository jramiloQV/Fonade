
CREATE PROCEDURE spActualizarEvaluacionesDeRiesgo
(
	@Id_Riesgo INT,
	@CodProyecto INT,
	@CodConvocatoria INT,
	@Riesgo VARCHAR(500),
	@Mitigacion VARCHAR(500)
)
AS
UPDATE dbo.EvaluacionRiesgo
SET CodProyecto = @CodProyecto,
	CodConvocatoria = @CodConvocatoria,
	Riesgo = @Riesgo,
	Mitigacion = @Mitigacion
WHERE Id_Riesgo = @Id_Riesgo