

-- ==========================================================================================
-- Entity Name:	sp_EvaluacionProyectoIndicador_Insert
-- Author:	Sergio Espinosa
-- Create date:	08/02/2014 10:05:29 a. m.
-- Description:	This stored procedure is intended for inserting values to EvaluacionProyectoIndicador table
-- ==========================================================================================
Create Procedure sp_EvaluacionProyectoIndicador_Insert
	@codProyecto int,
	@codConvocatoria int,
	@Descripcion varchar(255),
	@Tipo char(1),
	@Valor float,
	@Protegido bit
As
Begin
	Insert Into EvaluacionProyectoIndicador
		([codProyecto],[codConvocatoria],[Descripcion],[Tipo],[Valor],[Protegido])
	Values
		(@codProyecto,@codConvocatoria,@Descripcion,@Tipo,@Valor,@Protegido)

	Declare @ReferenceID int
	Select @ReferenceID = @@IDENTITY

	Return @ReferenceID

End