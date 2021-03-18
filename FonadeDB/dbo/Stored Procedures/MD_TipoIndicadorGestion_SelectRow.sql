

-- ==========================================================================================
-- Entity Name:	MD_TipoIndicadorGestion_SelectRow
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 3:53:03 p. m.
-- Description:	This stored procedure is intended for selecting a specific row from TipoIndicadorGestion table
-- ==========================================================================================
Create Procedure MD_TipoIndicadorGestion_SelectRow
	@Id_TipoIndicador smallint
As
Begin
	Select 
		[Id_TipoIndicador],
		[nomTipoIndicador],
		[Protegido]
	From TipoIndicadorGestion
	Where
		[Id_TipoIndicador] = @Id_TipoIndicador
End