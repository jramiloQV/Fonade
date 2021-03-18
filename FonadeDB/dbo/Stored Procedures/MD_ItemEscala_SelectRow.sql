

-- ==========================================================================================
-- Entity Name:	MD_ItemEscala_SelectRow
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 10:34:57 a. m.
-- Description:	This stored procedure is intended for selecting a specific row from ItemEscala table
-- ==========================================================================================
Create Procedure MD_ItemEscala_SelectRow
@CodItem int
As
Begin
	Select 
		[CodItem],
		[Texto],
		[Puntaje]
	From ItemEscala
	Where [CodItem]= @CodItem

End