

-- ==========================================================================================
-- Entity Name:	MD_Item_DeleteRow
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 10:34:53 a. m.
-- Description:	This stored procedure is intended for deleting a specific row from Item table
-- ==========================================================================================
Create Procedure MD_Item_DeleteRow
	@Id_Item int
As
Begin
	Delete Item
	Where
		[Id_Item] = @Id_Item
End