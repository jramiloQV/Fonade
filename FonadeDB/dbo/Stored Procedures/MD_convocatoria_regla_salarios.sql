CREATE PROCEDURE [dbo].[MD_convocatoria_regla_salarios]

	@CodConvocatoriaR int
	,@ExpresionLogicaR varchar(5)
	,@EmpleosGenerados1R int
	,@EmpleosGenerados2R int=null
	,@SalariosAPrestarR int
	,@NoReglaR int
	,@caso varchar(10)

AS
BEGIN

	IF @caso='Create'

		begin

			insert into ConvocatoriaReglaSalarios (CodConvocatoria,ExpresionLogica,EmpleosGenerados1,EmpleosGenerados2,SalariosAPrestar,NoRegla)
			values (@CodConvocatoriaR,@ExpresionLogicaR,@EmpleosGenerados1R,@EmpleosGenerados2R,@SalariosAPrestarR,@NoReglaR)

		end


	IF @caso='Update'

		begin

			UPDATE ConvocatoriaReglaSalarios
			SET ExpresionLogica = @ExpresionLogicaR
			,EmpleosGenerados1 = @EmpleosGenerados1R
			,EmpleosGenerados2 = @EmpleosGenerados2R
			,SalariosAPrestar = @SalariosAPrestarR
			WHERE CodConvocatoria=@CodConvocatoriaR
			and NoRegla=@NoReglaR
		
		end

	IF @caso='Delete'

		begin

			delete from ConvocatoriaReglaSalarios
			WHERE CodConvocatoria=@CodConvocatoriaR
			and NoRegla=@NoReglaR
		
		end
END