-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [MD_Insertar_Actualizar_EvaluacionIndicadorGestion]
	-- Add the parameters for the stored procedure here
		@_Id_IndicadorGestion int,
		@_CodProyecto int,
		@_CodConvocatoria int,
		@_Aspecto varchar(300),
		@_FechaSeguimiento varchar(60),
		@_Numerador  varchar(60),
		@_Denominador varchar(60),
		@_Descripcion varchar(60),
		@_RangoAceptable tinyint,
		@_caso varchar(100)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @_caso = 'CREATE'
		BEGIN
			INSERT INTO [EvaluacionIndicadorGestion]
				([CodProyecto],[CodConvocatoria],[Aspecto],[FechaSeguimiento],[Numerador],[Denominador],[Descripcion],[RangoAceptable])
			VALUES
				(@_CodProyecto,@_CodConvocatoria,@_Aspecto,@_FechaSeguimiento,@_Numerador,@_Denominador,@_Descripcion,@_RangoAceptable)
		END
	IF @_caso = 'UPDATE'
		BEGIN
			UPDATE [EvaluacionIndicadorGestion]
			SET
			[Aspecto] = @_Aspecto,
			[FechaSeguimiento] = @_FechaSeguimiento,
			[Numerador] = @_Numerador,
			[Denominador] = @_Denominador,
			[Descripcion] = @_Descripcion,
			[RangoAceptable] = @_RangoAceptable
			WHERE
			[Id_IndicadorGestion] = @_Id_IndicadorGestion
		END
END