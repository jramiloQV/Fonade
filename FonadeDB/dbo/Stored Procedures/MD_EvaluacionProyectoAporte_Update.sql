

-- ==========================================================================================
-- Entity Name:	MD_EvaluacionProyectoAporte_Update
-- Author:	Sergio Espinosa
-- Create date:	10/02/2014 2:50:06 p. m.
-- Description:	This stored procedure is intended for updating EvaluacionProyectoAporte table
-- ==========================================================================================
Create Procedure MD_EvaluacionProyectoAporte_Update
	@Id_Aporte int,
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
	Update EvaluacionProyectoAporte
	Set
		[CodProyecto] = @CodProyecto,
		[CodConvocatoria] = @CodConvocatoria,
		[CodTipoIndicador] = @CodTipoIndicador,
		[Nombre] = @Nombre,
		[Detalle] = @Detalle,
		[Solicitado] = @Solicitado,
		[Recomendado] = @Recomendado,
		[Protegido] = @Protegido
	Where		
		[Id_Aporte] = @Id_Aporte

End