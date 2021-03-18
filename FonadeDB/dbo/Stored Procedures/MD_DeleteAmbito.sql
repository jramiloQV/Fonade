
CREATE PROCEDURE [dbo].[MD_DeleteAmbito]

	@caso varchar(10)
	,@IdAmbito int
	
	
AS
BEGIN
	BEGIN

	
		iF @caso = 'Delete'
		begin
		Delete from Ambito 

			WHERE Id_Ambito = @IdAmbito
		end
END
END