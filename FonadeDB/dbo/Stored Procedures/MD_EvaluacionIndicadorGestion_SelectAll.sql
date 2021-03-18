

-- ==========================================================================================
-- Entity Name:	MD_EvaluacionIndicadorGestion_SelectAll
-- Author:	Sergio Espinosa
-- Create date:	11/02/2014 2:14:25 p. m.
-- Description:	This stored procedure is intended for selecting all rows from EvaluacionIndicadorGestion table
-- ==========================================================================================
Create Procedure MD_EvaluacionIndicadorGestion_SelectAll
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
End