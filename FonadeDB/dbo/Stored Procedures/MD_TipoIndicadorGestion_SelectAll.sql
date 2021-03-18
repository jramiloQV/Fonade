

-- ==========================================================================================
-- Entity Name:	MD_TipoIndicadorGestion_SelectAll
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 3:53:03 p. m.
-- Description:	This stored procedure is intended for selecting all rows from TipoIndicadorGestion table
-- ==========================================================================================
Create Procedure MD_TipoIndicadorGestion_SelectAll
As
Begin
	Select 
		[Id_TipoIndicador],
		[nomTipoIndicador],
		[Protegido]
	From TipoIndicadorGestion
End