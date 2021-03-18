
CREATE PROCEDURE [dbo].[MD_Update_JefeUnidad]
	@icfes varchar(25),
	@fechaderegistro smalldatetime,
	@telefonoInst varchar(40),
	@faxInst varchar(20),
	@web varchar(100),
	@ciudadInst int,
	@idInstitucion int,
	@cargo varchar(100),
	@telefono varchar(100),
	@fax varchar(100),
	@ciudad int,
	@idcontacto int
AS

BEGIN

	UPDATE Institucion
	SET RegistroIcfes=@icfes
	,FechaRegistro=@fechaderegistro
	,Telefono=@telefonoInst
	,Fax=@faxInst
	,WebSite=@web
	,CodCiudad=@ciudadInst

	where Id_Institucion=@idInstitucion

	update Contacto
	set Cargo=@cargo
	,Telefono=@telefono
	,Fax=@fax
	,LugarExpedicionDI=@ciudad
	,fechaActualizacion=getdate()
	where Id_Contacto=@idcontacto

END