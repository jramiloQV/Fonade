
CREATE PROCEDURE [dbo].[MD_DeleteVariable]
	@caso varchar(10)
	,@IdVariable int
	
	
AS
BEGIN
	BEGIN

	IF @caso='Delete'

		begin

DELETE px FROM VARIABLE px
INNER JOIN TIPOVARIABLE p ON p.Id_TipoVariable = px.CodTipoVariable
AND px.id_Variable=@IdVariable


	
		
		end
END
END

select * from tipovariable