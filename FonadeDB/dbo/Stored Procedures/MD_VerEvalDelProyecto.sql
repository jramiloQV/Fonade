
CREATE PROCEDURE [dbo].[MD_VerEvalDelProyecto]

	@CodProyecto int

AS

BEGIN

	Select codcontacto, Nombres + ' ' + Apellidos as nombre, Email from proyectocontacto
	inner join Contacto on Id_Contacto = CodContacto
	where proyectocontacto.inactivo=0 and CodProyecto= @CodProyecto
	and CodRol = 4

END