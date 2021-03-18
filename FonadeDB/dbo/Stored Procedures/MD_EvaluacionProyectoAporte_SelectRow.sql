

-- ==========================================================================================
-- Entity Name:	MD_EvaluacionProyectoAporte_SelectRow
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 2:50:06 p. m.
-- Description:	This stored procedure is intended for selecting a specific row from EvaluacionProyectoAporte table
-- ==========================================================================================
Create Procedure MD_EvaluacionProyectoAporte_SelectRow
	@Id_Aporte int
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
	Where
		[Id_Aporte] = @Id_Aporte
End