-- ==========================================================================================
-- Entity Name:	sp_EvaluacionProyectoIndicador_DeleteRow
-- Author:	Sergio Espinosa
-- Create date:	08/02/2014 10:05:29 a. m.
-- Description:	This stored procedure is intended for deleting a specific row from EvaluacionProyectoIndicador table
-- ==========================================================================================
Create Procedure sp_EvaluacionProyectoIndicador_DeleteRow
	@id_Indicador int
As
Begin
	Delete EvaluacionProyectoIndicador
	Where
		[id_Indicador] = @id_Indicador
End