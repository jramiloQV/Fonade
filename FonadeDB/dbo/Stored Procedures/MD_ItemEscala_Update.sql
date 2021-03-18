

-- ==========================================================================================
-- Entity Name:	MD_ItemEscala_Update
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 10:34:57 a. m.
-- Description:	This stored procedure is intended for updating ItemEscala table
-- ==========================================================================================
Create Procedure MD_ItemEscala_Update
	@CodItem int,
	@Texto varchar(255),
	@Puntaje smallint
As
Begin
	Update ItemEscala
	Set
		[CodItem] = @CodItem,
		[Texto] = @Texto,
		[Puntaje] = @Puntaje
	Where		[CodItem] = @CodItem

End