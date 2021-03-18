
create PROCEDURE [dbo].[MD_Update_UsGeneral]

	@cargo varchar(100),
	@telefono varchar(100),
	@fax varchar(100),
	@idcontacto int
AS

BEGIN

	update Fonade.dbo.Contacto
	set Cargo=@cargo
	,Telefono=@telefono
	,Fax=@fax
	,fechaActualizacion=getdate()
	where Id_Contacto=@idcontacto

END