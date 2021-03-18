
CREATE PROCEDURE [dbo].[MD_InsertUpdateAmbito]

	@caso varchar(10)
	,@idAmbito int
	,@nomAmbito varchar(100)
	
AS
BEGIN
	BEGIN

	IF @caso='Create'

		begin

				Insert into Ambito
			(NomAmbito) 
			values (@nomAmbito)


		end

	IF @caso='Update'

		begin

			Update Ambito set nomAmbito = @nomAmbito

			WHERE id_ambito = @idAmbito

	
		
		end
END
END