
CREATE PROCEDURE [dbo].[MD_DesactivarCoordinadorEvaluador]

	 @CodEvaluador int
	,@CONST_RolCoordinadorEvaluador int
	,@fecFin datetime=null
	,@Motivo varchar(250)

AS

BEGIN

		UPDATE Contacto SET Inactivo=1 WHERE Id_Contacto=@CodEvaluador

		UPDATE ProyectoContacto SET Inactivo=1 , FechaFin=getdate() 
		WHERE Inactivo=0 AND codRol=@CONST_RolCoordinadorEvaluador AND codContacto=@CodEvaluador

		UPDATE Evaluador SET codCoordinador=Null WHERE codCoordinador=@CodEvaluador
		
		INSERT INTO ContactoDesactivacion (CodContacto, FechaInicio, FechaFin, Comentario) 
		VALUES (@codEvaluador, getDate(), @fecFin, @Motivo)

END