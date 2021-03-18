CREATE PROCEDURE [dbo].[MD_InsertUpdateEvaluacionFlujoCajaObs]
	@CodProyecto int,
	@CodConvocatoria int,
	@ConclusionesFinan varchar(1000)
AS
BEGIN
	
	Declare @conteo int

	SELECT @conteo=COUNT(ConclusionesFinancieras)
	FROM evaluacionobservacion 
	WHERE 
		CodProyecto = @CodProyecto 
		AND CodConvocatoria = @CodConvocatoria

	IF @conteo=0

		begin
	
			INSERT INTO evaluacionobservacion 
			(
				CodProyecto
				,CodConvocatoria
				,Actividades
				,ProductosServicios
				,EstrategiaMercado
				,ProcesoProduccion
				,EstructuraOrganizacional
				,TamanioLocalizacion
				,Generales
				,ConclusionesFinancieras
			) 
			Values
			(
				@CodProyecto
				,@CodConvocatoria
				,''
				,''
				,''
				,''
				,''
				,''
				,''
				,@ConclusionesFinan
			)

		end

	ELSE

		begin
		
			UPDATE evaluacionobservacion 
			SET 
				ConclusionesFinancieras =  @ConclusionesFinan
			WHERE 
				CodProyecto = @CodProyecto 
				AND CodConvocatoria = 	@CodConvocatoria		

		end

END