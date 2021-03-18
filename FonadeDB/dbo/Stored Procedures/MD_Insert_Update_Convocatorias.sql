
CREATE PROCEDURE [dbo].[MD_Insert_Update_Convocatorias]

	  @NomConvocatoriaV varchar(80)
	, @DescripcionV varchar(500)
	, @FechaInicioV varchar(20)
	, @FechaFinV varchar(20)
	, @PresupuestoV float 
	, @MinimoPorPlanV float
	, @PublicadoV bit
	, @codContactoV int
	, @EncargoFiduciarioV varchar(20)
	, @CodConvenioV int
	, @idConvocatoriaV int
	, @caso varchar(10)
AS

BEGIN

	IF @caso='Create'

		begin

			INSERT Convocatoria (NomConvocatoria, Descripcion, FechaInicio, FechaFin, Presupuesto, MinimoPorPlan, Publicado, codContacto, EncargoFiduciario, CodConvenio,IdVersionProyecto)
			--VALUES ( @NomConvocatoriaV, @DescripcionV, CONVERT(SMALLDATETIME,@FechaInicioV,110), CONVERT(SMALLDATETIME,@FechaFinV,110), @PresupuestoV, @MinimoPorPlanV, @PublicadoV, @codContactoV, @EncargoFiduciarioV, @CodConvenioV)
			VALUES ( @NomConvocatoriaV, @DescripcionV, cast(@FechaInicioV as datetime), cast(@FechaFinV AS SMALLDATETIME), @PresupuestoV, @MinimoPorPlanV, @PublicadoV, @codContactoV, @EncargoFiduciarioV, @CodConvenioV,2)

		end


	IF @caso='Update1'

		begin

			UPDATE Convocatoria
			SET
			Descripcion = @DescripcionV,
			NomConvocatoria = @NomConvocatoriaV,
			FechaInicio = @FechaInicioV,
			MinimoPorPlan = @MinimoPorPlanV,
			Publicado = @PublicadoV,
			FechaFin = Cast(@FechaFinV as smalldatetime),
			Presupuesto = @PresupuestoV,
			codContacto = @codContactoV,
			CodConvenio = @CodConvenioV,
			EncargoFiduciario = @EncargoFiduciarioV
			WHERE id_Convocatoria= @idConvocatoriaV
		
		end


	IF @caso='Update2'

		begin

			UPDATE Convocatoria
			SET
			FechaFin = @FechaFinV,
			Presupuesto = @PresupuestoV,
			codContacto = @codContactoV,
			CodConvenio = @CodConvenioV,
			EncargoFiduciario = @EncargoFiduciarioV
			WHERE id_Convocatoria= @idConvocatoriaV
		
		end
END