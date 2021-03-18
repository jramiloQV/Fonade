-- ==========================================================================================
-- Entity Name:	MD_EvaluacionIndicadorGestion_DeleteRow
-- Author:	Sergio Espinosa
-- Create date:	11/02/2014 2:14:26 p. m.
-- Description:	This stored procedure is intended for deleting a specific row from EvaluacionIndicadorGestion table
-- ==========================================================================================
Create Procedure MD_EvaluacionIndicadorGestion_DeleteRow
	@Id_IndicadorGestion int
As
Begin
	Delete EvaluacionIndicadorGestion
	Where
		[Id_IndicadorGestion] = @Id_IndicadorGestion

End