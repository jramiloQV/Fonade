
create PROCEDURE [dbo].[MD_InsertUpdateAmbitos]

	@caso varchar(10)

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

			Update Ambito set NomAmbito = @nomAmbito

			WHERE @nomAmbito = @nomAmbito

	
		
		end
END
END