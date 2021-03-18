
CREATE PROCEDURE [dbo].[MD_MostrarEvaluacionRubroProyecto]

	@CodProyecto int,
	@CodCOnvocatoria int


AS

BEGIN

	SELECT Id_EvaluacionRubroProyecto, Descripcion 
	FROM EvaluacionRubroProyecto 
	WHERE codProyecto = @CodProyecto AND codConvocatoria = @CodCOnvocatoria

END