

-- ==========================================================================================
-- Entity Name:	MD_TipoIndicadorGestion_Update
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 3:53:03 p. m.
-- Description:	This stored procedure is intended for updating TipoIndicadorGestion table
-- ==========================================================================================
Create Procedure MD_TipoIndicadorGestion_Update
	@Id_TipoIndicador smallint,
	@nomTipoIndicador varchar(80),
	@Protegido bit
As
Begin
	Update TipoIndicadorGestion
	Set
		[nomTipoIndicador] = @nomTipoIndicador,
		[Protegido] = @Protegido
	Where		
		[Id_TipoIndicador] = @Id_TipoIndicador

End