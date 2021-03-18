

-- ==========================================================================================
-- Entity Name:	MD_EvaluacionProyectoAporte_DeleteRow
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 2:50:06 p. m.
-- Description:	This stored procedure is intended for deleting a specific row from EvaluacionProyectoAporte table
-- ==========================================================================================
Create Procedure MD_EvaluacionProyectoAporte_DeleteRow
	@Id_Aporte int
As
Begin
	Delete EvaluacionProyectoAporte
	Where
		[Id_Aporte] = @Id_Aporte

End