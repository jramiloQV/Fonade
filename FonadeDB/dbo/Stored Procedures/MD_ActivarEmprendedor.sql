create PROCEDURE MD_ActivarEmprendedor
	@id_contactoQ int,
	@NombreQ varchar(100),
	@ApellidoQ varchar(100),
	@emailQ varchar(100),
	@IdentificacionQ int,
	@direccionQ varchar(120),
	@telefonoQ varchar(100),
	@id_contactoLogQ int
AS

BEGIN
	Declare @Conteo int
	select @Conteo = count(Email) from contacto where email=@emailQ and id_contacto <> @id_contactoQ

	if @Conteo = 0
	begin
		Update contacto set Inactivo=0, nombres=@NombreQ, apellidos=@ApellidoQ, Email=@emailQ, identificacion=@IdentificacionQ,
		direccion=@direccionQ,telefono=@telefonoQ
		where id_contacto=@id_contactoQ

		insert into ContactoReActivacion (CodContacto, FechaReActivacion, CodContactoQReActiva)
		values(@id_contactoQ, getdate(),@id_contactoLogQ)

		insert into ContactoActualizoReactivacion (CodContacto,ActualizoDatos,CambioClave, FechaReActivacion)
		values(@id_contactoQ,0,0, getdate())
	end
	return @Conteo
END