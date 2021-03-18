/*

	Nombre: MD_prActualizarTabEval.
	Fecha: 17:02 24/06/2014
	Descripción: Procedimiento almacenado para guardar la última actualización sobre la
	evaluación.

*/
CREATE PROCEDURE [MD_prActualizarTabEval]
(
	@CodTabEvaluacion INT, 
	@CodProyecto INT, 
	@CodConvocatoria INT, 
	@CodContacto INT,
	@Caso VARCHAR(10),
	@FechaModificacion DATETIME
)
AS
IF @Caso = 'INSERT'
BEGIN
	INSERT INTO tabEvaluacionProyecto (CodTabEvaluacion, CodProyecto, CodConvocatoria, CodContacto, FechaModificacion)
	VALUES (@CodTabEvaluacion,@CodProyecto, @CodConvocatoria, @CodContacto, @FechaModificacion)--getdate())
END

IF @Caso = 'UPDATE'
BEGIN
	UPDATE tabEvaluacionProyecto
	SET codcontacto = @CodContacto,
	fechamodificacion = @FechaModificacion --getdate()
    WHERE codTabEvaluacion = @CodTabEvaluacion and Codproyecto=@CodProyecto
	and codConvocatoria=@CodConvocatoria
END