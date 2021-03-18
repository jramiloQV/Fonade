
CREATE PROCEDURE [dbo].[MD_InsertUpdateTipoVariable]
	@caso varchar(10)
	,@IdTipoVariable int
	,@nomTipoVariable varchar(100)
	
AS
BEGIN
	BEGIN

	IF @caso='Create'

		begin

			Insert into TipoVariable
			(NomTipoVariable) 
			values (@nomTipoVariable)


		end

	IF @caso='Update'

		begin

			Update TipoVariable set NomTipoVariable = @nomTipoVariable

			WHERE Id_TipoVariable = @IdTipoVariable

	
		
		end
END
END