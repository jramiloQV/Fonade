

-- ==========================================================================================
-- Entity Name:	sp_EvaluacionProyectoIndicador_Update
-- Author:	Sergio Espinosa
-- Create date:	08/02/2014 10:05:29 a. m.
-- Description:	This stored procedure is intended for updating EvaluacionProyectoIndicador table
-- ==========================================================================================
CREATE Procedure [dbo].[sp_EvaluacionProyectoIndicador_Update]
	@id_Indicador int,
	@codProyecto int,
	@codConvocatoria int,
	@Descripcion varchar(255),
	@Tipo char(1),
	@Valor float,
	@Protegido bit
As
Begin
	Update EvaluacionProyectoIndicador
	Set
		[codProyecto] = @codProyecto,
		[codConvocatoria] = @codConvocatoria,
		[Descripcion] = @Descripcion,
		[Tipo] = @Tipo,
		[Valor] = @Valor,
		[Protegido] = @Protegido
	Where		
		[id_Indicador] = @id_Indicador

End