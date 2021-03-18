

-- ==========================================================================================
-- Entity Name:	MD_EvaluacionIndicadorGestion_Update
-- Author:	Sergio Espinosa
-- Create date:	11/02/2014 2:14:26 p. m.
-- Description:	This stored procedure is intended for updating EvaluacionIndicadorGestion table
-- ==========================================================================================
Create Procedure MD_EvaluacionIndicadorGestion_Update
	@Id_IndicadorGestion int,
	@CodProyecto int,
	@CodConvocatoria int,
	@Aspecto varchar(300),
	@FechaSeguimiento varchar(60),
	@Numerador varchar(100),
	@Denominador varchar(100),
	@Descripcion varchar(300),
	@RangoAceptable tinyint
As
Begin
	Update EvaluacionIndicadorGestion
	Set
		[CodProyecto] = @CodProyecto,
		[CodConvocatoria] = @CodConvocatoria,
		[Aspecto] = @Aspecto,
		[FechaSeguimiento] = @FechaSeguimiento,
		[Numerador] = @Numerador,
		[Denominador] = @Denominador,
		[Descripcion] = @Descripcion,
		[RangoAceptable] = @RangoAceptable
	Where		
		[Id_IndicadorGestion] = @Id_IndicadorGestion

End