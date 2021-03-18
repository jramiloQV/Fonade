

-- ==========================================================================================
-- Entity Name:	MD_Item_Insert
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 10:34:52 a. m.
-- Description:	This stored procedure is intended for inserting values to Item table
-- ==========================================================================================
Create Procedure MD_Item_Insert
	@NomItem varchar(255),
	@CodTabEvaluacion smallint,
	@Protegido bit
As
Begin
	Insert Into Item
		([NomItem],[CodTabEvaluacion],[Protegido])
	Values
		(@NomItem,@CodTabEvaluacion,@Protegido)

	Declare @ReferenceID int
	Select @ReferenceID = @@IDENTITY

	Return @ReferenceID

End