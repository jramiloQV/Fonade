
Create PROCEDURE [dbo].[MD_DeleteTipoVariable]
	@caso varchar(10)
	,@IdTipoVariable int	
AS
BEGIN
	BEGIN
	IF @caso='Delete'
		begin
			delete from TipoVariable 
			WHERE Id_TipoVariable = @IdTipoVariable		
		end
END
END