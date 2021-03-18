

-- ==========================================================================================
-- Entity Name:	MD_EvaluacionEvaluador_SelectAll
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 9:53:15 a. m.
-- Description:	This stored procedure is intended for selecting all rows from EvaluacionEvaluador table
-- ==========================================================================================
Create Procedure MD_EvaluacionEvaluador_SelectAll
As
Begin
	Select 
		[CodProyecto],
		[CodConvocatoria],
		[CodItem],
		[Puntaje]
	From EvaluacionEvaluador
End