
CREATE PROCEDURE [dbo].[MD_InsertUpdateVariable]
	@caso varchar(10)
	,@IdVariable int
	,@nomVariable varchar(100)
	
AS
BEGIN
	BEGIN

	IF @caso='Create'

		begin

			Insert into Variable
			(NomVariable) 
			values (@nomVariable)


		end

	IF @caso='Update'

		begin

			Update Variable set NomVariable = @nomVariable

			WHERE Id_Variable = @IdVariable

	
		
		end

				

	IF @caso='Delete'

		begin

	DELETE FROM Variable WHERE Id_Variable  = @IdVariable
	
		end
	
END
END