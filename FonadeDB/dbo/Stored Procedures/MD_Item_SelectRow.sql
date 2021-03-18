

-- ==========================================================================================
-- Entity Name:	MD_Item_SelectRow
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 10:34:52 a. m.
-- Description:	This stored procedure is intended for selecting a specific row from Item table
-- ==========================================================================================
Create Procedure MD_Item_SelectRow
	@Id_Item int
As
Begin
	Select 
		[Id_Item],
		[NomItem],
		[CodTabEvaluacion],
		[Protegido]
	From Item
	Where
		[Id_Item] = @Id_Item
End