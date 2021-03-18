

-- ==========================================================================================
-- Entity Name:	MD_ItemEscala_Insert
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 10:34:57 a. m.
-- Description:	This stored procedure is intended for inserting values to ItemEscala table
-- ==========================================================================================
Create Procedure MD_ItemEscala_Insert
	@CodItem int,
	@Texto varchar(255),
	@Puntaje smallint
As
Begin
	Insert Into ItemEscala
		([CodItem],[Texto],[Puntaje])
	Values
		(@CodItem,@Texto,@Puntaje)

End