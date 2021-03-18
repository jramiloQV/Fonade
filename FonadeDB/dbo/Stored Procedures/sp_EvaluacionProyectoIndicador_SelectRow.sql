

-- ==========================================================================================
-- Entity Name:	sp_EvaluacionProyectoIndicador_SelectRow
-- Author:	Sergio Espinosa
-- Create date:	08/02/2014 10:05:29 a. m.
-- Description:	This stored procedure is intended for selecting a specific row from EvaluacionProyectoIndicador table
-- ==========================================================================================
Create Procedure sp_EvaluacionProyectoIndicador_SelectRow
	@id_Indicador int
As
Begin
	Select 
		[id_Indicador],
		[codProyecto],
		[codConvocatoria],
		[Descripcion],
		[Tipo],
		[Valor],
		[Protegido]
	From EvaluacionProyectoIndicador
	Where
		[id_Indicador] = @id_Indicador
End