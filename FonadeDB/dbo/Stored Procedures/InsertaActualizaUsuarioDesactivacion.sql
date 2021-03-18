/* =============================================
Author:		Wilson Guasca
alter date: 2010-06-21 09:00
Description: Crea o actualiza un registro en la 
  tabla [ServicioUsuariosDesactivados].
============================================= */
CREATE PROCEDURE [dbo].[InsertaActualizaUsuarioDesactivacion]
	@CodServicioRegistroDesactivacion Int,
	@CodContacto Int,
	@FechaUltimoAcceso DateTime,
	@FechaNotificacion DateTime = NULL,
	@EnvioExitoso bit,
	@CodRegistro Int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF (NOT @CodRegistro IS NULL) 
	/*
		AND EXISTS (SELECT TOP 1 [Id_ServicioUsuariosDesactivados] 
					FROM [ServicioUsuariosDesactivados]
					 WHERE [Id_ServicioUsuariosDesactivados] = @CodRegistro)
	*/
	BEGIN
		UPDATE [ServicioUsuariosDesactivados] SET
			   [CodServicioRegistroDesactivacion] = @CodServicioRegistroDesactivacion
			   ,[CodContacto] = @CodContacto
			   ,[FechaUltimoAcceso] = @FechaUltimoAcceso
			   ,[FechaNotificacion] = @FechaNotificacion
			   ,[EnvioExitoso] = @EnvioExitoso
		 WHERE [Id_ServicioUsuariosDesactivados] = @CodRegistro
	END
	ELSE
	BEGIN
		INSERT INTO [ServicioUsuariosDesactivados]
			   ([CodServicioRegistroDesactivacion]
			   ,[CodContacto]
			   ,[FechaUltimoAcceso]
			   ,[FechaNotificacion]
			   ,[EnvioExitoso])
		 VALUES
			   (@CodServicioRegistroDesactivacion
			   ,@CodContacto
			   ,@FechaUltimoAcceso
			   ,@FechaNotificacion
			   ,@EnvioExitoso)
		SET @CodRegistro = IDENT_CURRENT('ServicioUsuariosDesactivados')
	END
	
	-- Retorno de resultados
	SELECT Cast(@CodRegistro As Int) As 'Id_Registro', @@ROWCOUNT As 'Afectados'
END