

-- ==========================================================================================
-- Entity Name:	MD_EvaluacionEvaluador_DeleteRow
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 9:53:15 a. m.
-- Description:	This stored procedure is intended for deleting a specific row from EvaluacionEvaluador table
-- ==========================================================================================
Create Procedure MD_EvaluacionEvaluador_DeleteRow
	@CodProyecto int,
	@CodConvocatoria int,
	@CodItem int
As
Begin
	Delete EvaluacionEvaluador
	Where
		[CodProyecto] = @CodProyecto
		and [CodConvocatoria] = @CodConvocatoria
		and [CodItem] = @CodItem
End