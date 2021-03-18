

-- ==========================================================================================
-- Entity Name:	MD_EvaluacionEvaluador_Insert
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 9:53:15 a. m.
-- Description:	This stored procedure is intended for inserting values to EvaluacionEvaluador table
-- ==========================================================================================
Create Procedure MD_EvaluacionEvaluador_Insert
	@CodProyecto int,
	@CodConvocatoria int,
	@CodItem int,
	@Puntaje smallint
As
Begin
	Insert Into EvaluacionEvaluador
		([CodProyecto],[CodConvocatoria],[CodItem],[Puntaje])
	Values
		(@CodProyecto,@CodConvocatoria,@CodItem,@Puntaje)

End