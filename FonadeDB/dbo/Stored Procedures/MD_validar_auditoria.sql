
CREATE PROCEDURE [dbo].[MD_validar_auditoria]
	
	@caso varchar(1)
AS

BEGIN

	declare @valorRetorno bit

	IF @caso='U'

		begin

			select @valorRetorno = Modificar from HistoricoAuditoria where ultima_configuracion = 1

		end


	IF @caso='I'

		begin

			select @valorRetorno = Insertar from HistoricoAuditoria where ultima_configuracion = 1
		
		end


	IF @caso='D'

		begin

			select @valorRetorno = Eliminar from HistoricoAuditoria where ultima_configuracion = 1
		
		end

	return @valorRetorno
END