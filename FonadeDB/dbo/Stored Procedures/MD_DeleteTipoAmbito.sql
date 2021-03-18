
Create PROCEDURE [dbo].[MD_DeleteTipoAmbito]

	@caso varchar(10)
	,@IdTipoAmbito int
	
	
AS
BEGIN
	BEGIN

	
		iF @caso = 'Delete'
		begin
		Delete from TipoAmbito 

			WHERE Id_TipoAmbito = @IdTipoAmbito
		end
END
END