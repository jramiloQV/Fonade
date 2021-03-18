
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RetornarInfoContacto]
	 @Email nvarchar(250) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id_Contacto, Nombres, Apellidos, CodTipoIdentificacion, Identificacion, Genero, FechaNacimiento, Cargo, Email, Direccion, Telefono, Fax, Experiencia, Dedicacion, HojaVida, Intereses,CodCiudad, Clave, Inactivo, InactivoAsignacion, CodTipoAprendiz, fechaCreacion, InformacionIncompleta, LugarExpedicionDI, fechaActualizacion from Contacto where Id_Contacto=@Email
END