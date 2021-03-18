

-- ==========================================================================================
-- Entity Name:	MD_EvaluacionProyectoAporte_Insert
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 2:50:06 p. m.
-- Description:	This stored procedure is intended for inserting values to EvaluacionProyectoAporte table
-- ==========================================================================================
CREATE Procedure [dbo].[MD_EvaluacionProyectoAporte_Insert]
	@CodProyecto int,
	@CodConvocatoria int,
	@CodTipoIndicador smallint,
	@Nombre varchar(255),
	@Detalle varchar(300),
	@Solicitado float,
	@Recomendado float,
	@Protegido bit
As
Begin
	Insert Into EvaluacionProyectoAporte
		([CodProyecto],[CodConvocatoria],[CodTipoIndicador],[Nombre],[Detalle],[Solicitado],[Recomendado],[Protegido],[idFuenteFinanciacion])
	Values
		(@CodProyecto,@CodConvocatoria,@CodTipoIndicador,@Nombre,@Detalle,@Solicitado,@Recomendado,@Protegido,1)

	Declare @ReferenceID int
	Select @ReferenceID = @@IDENTITY

	Return @ReferenceID

End