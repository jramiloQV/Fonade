
create PROCEDURE [dbo].[MD_Update_Asesor]
	@dedicacion char(10),
	@experiencia varchar(max),
	@hojadevida text,
	@intereses varchar(max),
	@ciudad int,
	@idcontacto int
AS

BEGIN

	Update Contacto 
	set Experiencia=@experiencia
	, Dedicacion=@dedicacion
	, HojaVida=@hojadevida
	, Intereses=@intereses
	, LugarExpedicionDI =@ciudad
	, fechaActualizacion=getdate()

	WHERE Id_Contacto =@idcontacto

END