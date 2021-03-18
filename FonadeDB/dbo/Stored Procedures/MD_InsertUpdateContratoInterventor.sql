
CREATE PROCEDURE [dbo].[MD_InsertUpdateContratoInterventor]

	@CodContacto int,
	@NumContrato int,
	@FechaInicio datetime,
	@FechaExpiracion datetime,
	@Motivo varchar(200),
	@IdInterventorContrato int,
	@caso varchar(10)
AS

BEGIN

	IF @caso='Create'

		begin
			
			INSERT INTO InterventorContrato (CodContacto,NumContrato,FechaInicio,FechaExpiracion)
			VALUES (@CodContacto, @NumContrato, @FechaInicio, @FechaExpiracion)

		end


	IF @caso='Update'

		begin

			UPDATE InterventorContrato
			SET FechaInicio = @FechaInicio,
			FechaExpiracion = @FechaExpiracion,
			Motivo = @Motivo
			
			WHERE Id_InterventorContrato = @IdInterventorContrato
		  and NumContrato = @NumContrato
		end

END