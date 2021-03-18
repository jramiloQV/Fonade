
CREATE PROCEDURE [dbo].[MD_DesactivarEvaluador]

	@CONST_Convocatoria int
	,@CONST_Evaluacion int
	,@CONST_RolEvaluador int
	,@CodEvaluador int
	,@CONST_RolCoordinadorEvaluador int
	,@fecFin datetime=null
	,@Motivo varchar(250)

AS

BEGIN

	UPDATE Contacto SET Inactivo=1 WHERE Id_Contacto=@CodEvaluador

	UPDATE Proyecto SET codEstado=@CONST_Convocatoria WHERE codestado = @CONST_Evaluacion and
	id_Proyecto in 
	(Select codProyecto FROM ProyectoContacto WHERE Inactivo=0 AND codRol=@CONST_RolEvaluador AND codContacto=@CodEvaluador)

	UPDATE ProyectoContacto SET Inactivo=1 , FechaFin=getdate() 
	from proyectocontacto pc, evaluador e 
	WHERE Inactivo=0 AND codRol= @CONST_RolCoordinadorEvaluador
	AND pc.codcontacto=codcoordinador and e.codcontacto= @CodEvaluador

	UPDATE ProyectoContacto SET Inactivo=1 , FechaFin=getdate() 
	WHERE Inactivo=0 AND codRol= @CONST_RolEvaluador AND codContacto= @CodEvaluador

	INSERT INTO ContactoDesactivacion (CodContacto, FechaInicio, FechaFin, Comentario) 
	VALUES (@codEvaluador, getDate(), @fecFin, @Motivo)

END