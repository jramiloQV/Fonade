CREATE PROCEDURE [dbo].[MD_historicoAuditoria]
     @AuditarActivoV bit
	,@InsertarV bit
	,@ModificarV bit
	,@EliminarV bit
	,@ultimaconfiguracionV bit
	,@UsuarioconfiguracionV int
	,@Condition varchar(20)
AS
BEGIN

	update HistoricoAuditoria
	set ultima_configuracion = 0

	Insert into HistoricoAuditoria
			   (Auditar_Activo
			   ,Insertar
			   ,Modificar
			   ,Eliminar
			   ,ultima_configuracion
			   ,fecha_configuracion
			   ,Usuario_configuracion)
	values
	(
		@AuditarActivoV
		,@InsertarV
		,@ModificarV
		,@EliminarV
		,@ultimaconfiguracionV
		,GETDATE()
		,@UsuarioconfiguracionV
	)

	EXEC [dbo].[DisableAllTriggers] @condicion = @Condition;
END