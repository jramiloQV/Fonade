

create PROCEDURE MD_Update_Emprendedor
	@fechanacimiento smalldatetime,
	@ciudadexpedicion int,
	@telefono varchar(100),
	@ciudad int,
	@idcontacto int,
	@genero char(1)
AS

BEGIN

	Update Contacto 
	set Genero = @genero
	, FechaNacimiento = @fechanacimiento
	, CodCiudad= @ciudad
	, Telefono=@telefono
	, LugarExpedicionDI=@ciudadexpedicion
	, fechaActualizacion=getdate()
	
	WHERE Id_Contacto =@idcontacto

END