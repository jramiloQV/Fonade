
CREATE PROCEDURE [dbo].[MD_InsertUpdateContratoEvaluador]

	@CodContacto int,
	@NumContrato int,
	@FechaInicio datetime,
	@FechaExpiracion datetime,
	@Motivo varchar(500),
	@IdEvaluadorContrato int,
	@caso varchar(10)
AS

BEGIN

	IF @caso='Create'

		begin
			
			INSERT INTO EvaluadorContrato (CodContacto,NumContrato,FechaInicio,FechaExpiracion)
			VALUES (@CodContacto, @NumContrato, @FechaInicio, @FechaExpiracion)

		end


	IF @caso='Update'

		begin

			UPDATE EvaluadorContrato
			SET FechaInicio = @FechaInicio,
			FechaExpiracion = @FechaExpiracion,
			Motivo = @Motivo
			WHERE id_EvaluadorContrato = @IdEvaluadorContrato
		
		end

END