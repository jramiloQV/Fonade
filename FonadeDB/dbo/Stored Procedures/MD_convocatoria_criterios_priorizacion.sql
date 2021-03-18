
CREATE PROCEDURE [dbo].[MD_convocatoria_criterios_priorizacion]

	 --@Id_usuario varchar(20)
	@IdConvocatoria int
	,@IdCriterioPriorizacion int
	,@parametro varchar(50)=null
	,@incidencias float
	,@caso varchar(10)


AS

BEGIN

--DECLARE  @infoC VARBINARY(128) 
--SET @infoC=CAST(@Id_usuario AS VARBINARY(128)) 
--SET CONTEXT_INFO @infoC

	IF @caso='Create'

		begin

			insert into ConvocatoriaCriterioPriorizacion 
			values (@IdConvocatoria,@IdCriterioPriorizacion,null,0)

		end


	IF @caso='Update'

		begin

			update ConvocatoriaCriterioPriorizacion 
			set Parametros = @parametro
			, Incidencia = @incidencias
			where codcriteriopriorizacion = @IdCriterioPriorizacion
			and Codconvocatoria=@IdConvocatoria
		
		end

	IF @caso='Delete'

		begin

			delete from ConvocatoriaCriterioPriorizacion 
			where codcriteriopriorizacion = @IdCriterioPriorizacion
			and Codconvocatoria= @IdConvocatoria
		
		end
END