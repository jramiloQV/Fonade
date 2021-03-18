-- ==========================================================================================
-- Entity Name:	MD_ItemEscala_DeleteRow
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 10:34:57 a. m.
-- Description:	This stored procedure is intended for deleting a specific row from ItemEscala table
-- ==========================================================================================
Create Procedure MD_ItemEscala_DeleteRow
@CodItem int
As
Begin
	Delete ItemEscala
	Where[CodItem] = @CodItem


End