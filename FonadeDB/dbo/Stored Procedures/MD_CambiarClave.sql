
create PROCEDURE MD_CambiarClave
	@CodUsuario int,
	@nuevaclave varchar(20)
AS

BEGIN
	UPDATE Clave SET DebeCambiar=0,YaAvisoExpiracion=1 WHERE CodContacto=@CodUsuario

	INSERT INTO Clave(NomClave,FechaVigencia,FechaDebeActualizar,DebeCambiar,Estado,YaAvisoExpiracion,CodContacto)
	VALUES(@nuevaclave,null,null,0,0,0,@CodUsuario)

	UPDATE Contacto SET Clave=@nuevaclave, fechaCambioClave=getdate() WHERE Id_Contacto=@CodUsuario
END