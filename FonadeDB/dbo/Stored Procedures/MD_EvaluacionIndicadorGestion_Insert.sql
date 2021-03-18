

-- ==========================================================================================
-- Entity Name:	MD_EvaluacionIndicadorGestion_Insert
-- Author:	Sergio Espinosa
-- Create date:	11/02/2014 2:14:26 p. m.
-- Description:	This stored procedure is intended for inserting values to EvaluacionIndicadorGestion table
-- ==========================================================================================
Create Procedure MD_EvaluacionIndicadorGestion_Insert
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
	Insert Into EvaluacionIndicadorGestion
		([CodProyecto],[CodConvocatoria],[Aspecto],[FechaSeguimiento],[Numerador],[Denominador],[Descripcion],[RangoAceptable])
	Values
		(@CodProyecto,@CodConvocatoria,@Aspecto,@FechaSeguimiento,@Numerador,@Denominador,@Descripcion,@RangoAceptable)

	Declare @ReferenceID int
	Select @ReferenceID = @@IDENTITY

	Return @ReferenceID

End