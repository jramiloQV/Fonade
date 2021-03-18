
CREATE PROCEDURE [dbo].[MD_cargarProyectosEval]

	@CodContactoEval int
	,@CONST_RolEvaluador int

AS

BEGIN
	SELECT distinct NomProyecto, Sumario FROM Proyecto
	inner join proyectocontacto pc on id_proyecto=codproyecto and
	pc.inactivo=0 
	and Codrol= @CONST_RolEvaluador and pc.CodContacto= @CodContactoEval
END