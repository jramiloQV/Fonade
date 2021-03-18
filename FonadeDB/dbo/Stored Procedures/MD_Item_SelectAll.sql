

-- ==========================================================================================
-- Entity Name:	MD_Item_SelectAll
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 10:34:52 a. m.
-- Description:	This stored procedure is intended for selecting all rows from Item table
-- ==========================================================================================
Create Procedure MD_Item_SelectAll
As
Begin
	Select 
		[Id_Item],
		[NomItem],
		[CodTabEvaluacion],
		[Protegido]
	From Item
End