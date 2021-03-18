

CREATE PROCEDURE [dbo].[MD_CargarJefeUnidadMiPerfil]
	@id_contacto int
AS

BEGIN
	SELECT C.Identificacion as IdentificacionC, C.Nombres as NombresC, C.Apellidos as Apellidos,
	C.Email as EmailC, C.Cargo as CargoC, C.Telefono as TelefonoC, C.Fax as FaxC, isnull(C.LugarExpedicionDI, 111) as LugarExpedicionDIC, 
	I.CodTipoInstitucion as CodTipoInstitucionI, I.NomInstitucion as NomInstitucionI, I.Nit as NitI, 
	I.RegistroIcfes as RegistroIcfesI, I.FechaRegistro as FechaRegistroI,isnull(I.CodCiudad, 111)  as CodCiudadI, 
	I.Direccion as DireccionI, I.Telefono as TelefonoI, I.Fax as FaxI, I.Website as WebsiteI
	FROM Contacto C 
	inner join Institucion I on I.Id_Institucion= C.CodInstitucion
	WHERE Id_Contacto =@id_contacto
END