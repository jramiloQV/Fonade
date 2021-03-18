
CREATE PROCEDURE [dbo].[MD_InsertUpdateEvaluacionCentrales]
	@CodProyecto int,
	@CodConvocatoria int,
	@CentralRiesgo varchar(1000),
	@FechaCentral datetime
AS

BEGIN
	
	Declare @conteo int
	SELECT @conteo=Count(codProyecto) FROM EvaluacionObservacion WHERE codProyecto = @CodProyecto AND codConvocatoria = @CodConvocatoria

	IF @conteo=0

		begin
	
			INSERT INTO EvaluacionObservacion 
			(
				CodProyecto
				, CodConvocatoria
				, Actividades
				, ProductosServicios
				, EstrategiaMercado
				, ProcesoProduccion
				, EstructuraOrganizacional
				, TamanioLocalizacion
				, Generales
				, CentralesRiesgo
				, FechaCentralesRiesgo
			) 
			VALUES 
			(
				@CodProyecto
				, @CodConvocatoria
				, ''
				, ''
				, ''
				, ''
				, ''
				, ''
				, ''
				, @CentralRiesgo
				, @FechaCentral
			)

		end

	ELSE

		begin
	
			UPDATE EvaluacionObservacion 
			SET
				FechaCentralesRiesgo = @FechaCentral
				, CentralesRiesgo = @CentralRiesgo
			WHERE 
				codProyecto =  @CodProyecto
				AND codConvocatoria = @CodConvocatoria			

		end

END