

-- ==========================================================================================
-- Entity Name:	MD_EvaluacionIndicadorGestion_SelectRow
-- Author:	Sergio Espinosa
-- Create date:	11/02/2014 2:14:26 p. m.
-- Description:	This stored procedure is intended for selecting a specific row from EvaluacionIndicadorGestion table
-- ==========================================================================================
Create Procedure MD_EvaluacionIndicadorGestion_SelectRow
	@Id_IndicadorGestion int
As
Begin
	Select 
		[Id_IndicadorGestion],
		[CodProyecto],
		[CodConvocatoria],
		[Aspecto],
		[FechaSeguimiento],
		[Numerador],
		[Denominador],
		[Descripcion],
		[RangoAceptable]
	From EvaluacionIndicadorGestion
	Where
		[Id_IndicadorGestion] = @Id_IndicadorGestion
End