

-- ==========================================================================================
-- Entity Name:	MD_ItemEscala_SelectAll
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 10:34:57 a. m.
-- Description:	This stored procedure is intended for selecting all rows from ItemEscala table
-- ==========================================================================================
Create Procedure MD_ItemEscala_SelectAll
As
Begin
	Select 
		[CodItem],
		[Texto],
		[Puntaje]
	From ItemEscala
End