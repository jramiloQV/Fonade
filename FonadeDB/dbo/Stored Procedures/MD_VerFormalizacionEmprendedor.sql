create PROCEDURE [dbo].[MD_VerFormalizacionEmprendedor]
	@id_proyecto int,
	@CONST_RolEmprendedor int
AS

BEGIN

	SELECT C.Nombres as NombresQ4, C.Apellidos as ApellidosQ4, P.participacion AS participacionQ4, P.beneficiario AS beneficiarioQ4
	FROM ProyectoContacto P, contacto C 
	WHERE P.codcontacto=C.id_contacto AND P.codProyecto=@id_proyecto AND P.Inactivo=0 AND P.codRol=@CONST_RolEmprendedor
	ORDER BY C.Nombres, C.Apellidos

	
END