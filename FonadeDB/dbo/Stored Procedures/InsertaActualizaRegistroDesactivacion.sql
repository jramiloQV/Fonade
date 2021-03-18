
/* =============================================
Author:		Wilson Guasca
alter date: 2010-06-21 09:00
Description: Crea o actualiza un registro en la 
  tabla [ServicioRegistroDesactivacion].
============================================= */
CREATE PROCEDURE [dbo].[InsertaActualizaRegistroDesactivacion]
	@FechaReferencia DateTime,
	@InicioDesactivacion DateTime,
	@FinDesactivacion DateTime,
	@TotalDesactivados Int,
	@FechaProcesoNotificacion DateTime = NULL,
	@CodRegistro Int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF (NOT @CodRegistro IS NULL) 
	/*
		AND EXISTS (SELECT TOP 1 [Id_ServicioRegistroDesactivacion] 
					FROM [ServicioRegistroDesactivacion]
					 WHERE [Id_ServicioRegistroDesactivacion] = @CodRegistro)
	*/
	BEGIN
		UPDATE [ServicioRegistroDesactivacion] SET
			   [FechaReferencia] = @FechaReferencia
			   ,[InicioDesactivacion] = @InicioDesactivacion
			   ,[FinDesactivacion] = @FinDesactivacion
			   ,[TotalDesactivados] = @TotalDesactivados
			   ,[FechaProcesoNotificacion] = @FechaProcesoNotificacion
		 WHERE [Id_ServicioRegistroDesactivacion] = @CodRegistro
	END
	ELSE
	BEGIN
		INSERT INTO [ServicioRegistroDesactivacion]
				   ([FechaReferencia]
				   ,[InicioDesactivacion]
				   ,[FinDesactivacion]
				   ,[TotalDesactivados]
				   ,[FechaProcesoNotificacion])
			 VALUES
				   (@FechaReferencia
				   ,@InicioDesactivacion
				   ,@FinDesactivacion
				   ,@TotalDesactivados
				   ,@FechaProcesoNotificacion)
		SET @CodRegistro = IDENT_CURRENT('ServicioRegistroDesactivacion')
	END
	
	-- Retorno de resultados
	SELECT Cast(@CodRegistro As Int) As 'Id_Registro', @@ROWCOUNT As 'Afectados'
END