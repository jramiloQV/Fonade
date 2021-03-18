CREATE PROCEDURE [dbo].[MD_cargarAsesorMiPerfil]
	@id_contacto int
AS

BEGIN
	SELECT Identificacion, Nombres, Apellidos, Email, Experiencia, replace(Dedicacion,' ','0') as Dedicacion, HojaVida, 
	Intereses, isnull(LugarExpedicionDI, 111) as LugarExpedicionDI 
	FROM Contacto 
	WHERE Id_Contacto =@id_contacto
END