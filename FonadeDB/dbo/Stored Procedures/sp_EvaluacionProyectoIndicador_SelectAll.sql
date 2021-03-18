

-- ==========================================================================================
-- Entity Name:	sp_EvaluacionProyectoIndicador_SelectAll
-- Author:	Sergio Espinosa
-- Create date:	08/02/2014 10:05:28 a. m.
-- Description:	This stored procedure is intended for selecting all rows from EvaluacionProyectoIndicador table
-- ==========================================================================================
CREATE Procedure [dbo].[sp_EvaluacionProyectoIndicador_SelectAll]
@codProyecto int,
@codConvocatoria int
 As
Begin
	Select 
		[id_Indicador],
		[Descripcion],
		[Valor],
		[Tipo]
	From EvaluacionProyectoIndicador
	where codProyecto = @codProyecto and codConvocatoria=  @codConvocatoria
End