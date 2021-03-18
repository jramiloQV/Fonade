
 CREATE PROCEDURE [dbo].[MD_DesactivarCoordinadorInterventor]

	 @CodInterventor int
	,@CONST_RolCoordinadorInterventor int
	,@fecFin datetime=null
	,@Motivo varchar(250)

AS

BEGIN

		UPDATE Contacto SET Inactivo=1 WHERE Id_Contacto=@CodInterventor

		UPDATE ProyectoContacto SET Inactivo=1 , FechaFin=getdate() 
		WHERE Inactivo=0 AND codRol=@CONST_RolCoordinadorinterventor AND codContacto=@CodInterventor

		UPDATE Evaluador SET codCoordinador=Null WHERE codCoordinador=@CodInterventor
		
		INSERT INTO ContactoDesactivacion (CodContacto, FechaInicio, FechaFin, Comentario) 
		VALUES (@CodInterventor, getDate(), @fecFin, @Motivo)

END