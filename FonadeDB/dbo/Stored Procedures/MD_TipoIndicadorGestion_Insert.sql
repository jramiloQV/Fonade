

-- ==========================================================================================
-- Entity Name:	MD_TipoIndicadorGestion_Insert
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 3:53:03 p. m.
-- Description:	This stored procedure is intended for inserting values to TipoIndicadorGestion table
-- ==========================================================================================
Create Procedure MD_TipoIndicadorGestion_Insert
	@Id_TipoIndicador smallint,
	@nomTipoIndicador varchar(80),
	@Protegido bit
As
Begin
	Insert Into TipoIndicadorGestion
		([Id_TipoIndicador],[nomTipoIndicador],[Protegido])
	Values
		(@Id_TipoIndicador,@nomTipoIndicador,@Protegido)

End