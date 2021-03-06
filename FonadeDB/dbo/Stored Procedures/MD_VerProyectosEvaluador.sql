
CREATE PROCEDURE [dbo].[MD_VerProyectosEvaluador]

	@CodContacto int, 
	@CONST_Evaluacion int
	
AS

BEGIN

	SELECT P.NomProyecto as proyecto, convert(varchar (50), PC.FechaInicio, 107) as fecha
	FROM Contacto C, Proyecto P, ProyectoContacto PC, Rol R
	WHERE PC.CodContacto = Id_Contacto
	AND PC.CodRol = R.Id_Rol
	AND PC.CodProyecto = P.Id_Proyecto
	AND PC.FechaFin IS NULL
	AND PC.Inactivo = 0
	AND Id_Contacto = @CodContacto And codestado=@CONST_Evaluacion ORDER BY P.Id_Proyecto

END