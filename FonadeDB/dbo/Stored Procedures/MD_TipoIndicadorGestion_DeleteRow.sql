

-- ==========================================================================================
-- Entity Name:	MD_TipoIndicadorGestion_DeleteRow
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 3:53:04 p. m.
-- Description:	This stored procedure is intended for deleting a specific row from TipoIndicadorGestion table
-- ==========================================================================================
Create Procedure MD_TipoIndicadorGestion_DeleteRow
	@Id_TipoIndicador smallint
As
Begin
	Delete TipoIndicadorGestion
	Where
		[Id_TipoIndicador] = @Id_TipoIndicador

End