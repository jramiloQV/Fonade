CREATE PROCEDURE [dbo].[MD_Actualizar_ConvocatoriaProyecto]
	-- Add the parameters for the stored procedure here
	@_CodConvocatoria int,
		@_CodProyecto int,
		@_Justificacion text,
		@_Viable bit,
		@_codevaluacionconceptos int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [ConvocatoriaProyecto]
			SET
				[Justificacion] = @_Justificacion
				,[Viable] = @_Viable
				,[codevaluacionconceptos] = @_codevaluacionconceptos
			WHERE
				[CodConvocatoria] = @_CodConvocatoria
				AND [CodProyecto] = @_CodProyecto
END