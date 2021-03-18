

-- ==========================================================================================
-- Entity Name:	MD_EvaluacionEvaluador_Update
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 9:53:15 a. m.
-- Description:	This stored procedure is intended for updating EvaluacionEvaluador table
-- ==========================================================================================
Create Procedure MD_EvaluacionEvaluador_Update
	@CodProyecto int,
	@CodConvocatoria int,
	@CodItem int,
	@Puntaje smallint
As
Begin
	Update EvaluacionEvaluador
	Set
		[Puntaje] = @Puntaje
	Where		
		[CodProyecto] = @CodProyecto
		and [CodConvocatoria] = @CodConvocatoria
		and [CodItem] = @CodItem

End