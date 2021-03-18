
CREATE PROCEDURE [dbo].[MD_InsertUpdateTipoAmbito]

	@caso varchar(10)
	,@IdTipoAmbito int
	,@nomTipoAmbito varchar(100)
	
AS
BEGIN
	BEGIN

	IF @caso='Create'

		begin

			Insert into TipoAmbito
			(NomTipoAmbito) 
			values (@nomTipoAmbito)


		end

	IF @caso='Update'

		begin

			Update TipoAmbito set NomTipoAmbito = @nomTipoAmbito

			WHERE Id_TipoAmbito = @IdTipoAmbito

	
		
		end
		iF @caso = 'Delete'
		begin
		Delete from TipoAmbito 

			WHERE Id_TipoAmbito = @IdTipoAmbito
		end
END
END