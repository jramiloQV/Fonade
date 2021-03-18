

-- ==========================================================================================
-- Entity Name:	MD_Item_Update
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 10:34:53 a. m.
-- Description:	This stored procedure is intended for updating Item table
-- ==========================================================================================
Create Procedure MD_Item_Update
	@Id_Item int,
	@NomItem varchar(255),
	@CodTabEvaluacion smallint,
	@Protegido bit
As
Begin
	Update Item
	Set
		[NomItem] = @NomItem,
		[CodTabEvaluacion] = @CodTabEvaluacion,
		[Protegido] = @Protegido
	Where		
		[Id_Item] = @Id_Item

End