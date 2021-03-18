
CREATE PROCEDURE [dbo].[MD_VerEvaluadoresDeCoordinador]

	@CodContacto int, 
	@CodEstado int
	
AS

BEGIN

	select distinct nombres + ' ' + apellidos as nombre, isnull(nomproyecto,'(Ninguno)') as nomproyecto 
	from contacto c 
	join evaluador e on id_contacto=e.codcontacto and codcoordinador= @CodContacto
	left join proyectocontacto pc on id_contacto=pc.codcontacto and  pc.inactivo=0 
	left join proyecto on id_proyecto=codproyecto and codestado = @CodEstado

END