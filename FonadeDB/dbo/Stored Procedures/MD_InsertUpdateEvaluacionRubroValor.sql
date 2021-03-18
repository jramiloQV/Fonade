
CREATE PROCEDURE [dbo].[MD_InsertUpdateEvaluacionRubroValor]
	@CodProyecto int,
	@CodConvocatoria int,
	@CodEvalRubroProy int,
	@Periodo int,
	@valor float
AS

BEGIN
	
	Declare @conteo int

	SELECT @conteo=count(Valor) 
	FROM EvaluacionRubroValor 
	WHERE CodEvaluacionRubroProyecto = @CodEvalRubroProy AND Periodo = @Periodo

	IF @conteo=0

		begin
	
			INSERT INTO EvaluacionRubroValor 
			(
				CodEvaluacionRubroProyecto
				, Periodo
				, Valor
			) 
			VALUES
			(
				@CodEvalRubroProy
				, @Periodo
				, @valor
			)

		end

	ELSE

		begin
		
			UPDATE EvaluacionRubroValor 
			SET 
				Valor =  @valor
			WHERE 
				CodEvaluacionRubroProyecto =  @CodEvalRubroProy
				AND Periodo = @Periodo			

		end

END