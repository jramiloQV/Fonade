

-- ==========================================================================================
-- Entity Name:	MD_EvaluacionProyectoAporte_SelectAll
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 2:50:05 p. m.
-- Description:	This stored procedure is intended for selecting all rows from EvaluacionProyectoAporte table
-- ==========================================================================================
Create Procedure MD_EvaluacionProyectoAporte_SelectAll
As
Begin
	Select 
		[Id_Aporte],
		[CodProyecto],
		[CodConvocatoria],
		[CodTipoIndicador],
		[Nombre],
		[Detalle],
		[Solicitado],
		[Recomendado],
		[Protegido]
	From EvaluacionProyectoAporte
End