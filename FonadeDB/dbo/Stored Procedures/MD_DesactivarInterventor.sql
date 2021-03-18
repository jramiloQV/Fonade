
CREATE PROCEDURE [dbo].[MD_DesactivarInterventor]

	@CONST_Convocatoria int
	,@CONST_Interventor int
	,@CONST_RolInterventor int
	,@CodInterventor int
	,@CONST_RolCoordinadorInterventor int
	,@fecFin datetime=null
	,@Motivo varchar(250)

AS

BEGIN

	UPDATE Contacto SET Inactivo=1 WHERE Id_Contacto=@CodInterventor


	UPDATE EmpresaInterventor SET Inactivo=1 , FechaFin=getdate() 
	WHERE Inactivo = 0 AND Rol = @CONST_RolInterventor  AND 
	codcontacto = @CodInterventor

	UPDATE EmpresaInterventor SET Inactivo = 1 , FechaFin = getdate()
	 WHERE Inactivo=0 AND Rol=@CONST_RolCoordinadorInterventor AND CodContacto =@CodInterventor


	INSERT INTO ContactoDesactivacion (CodContacto, FechaInicio, FechaFin, Comentario) 
	VALUES (@codInterventor, getDate(), @fecFin, @Motivo)

END