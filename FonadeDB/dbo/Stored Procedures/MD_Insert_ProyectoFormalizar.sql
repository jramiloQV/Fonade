
CREATE PROCEDURE [dbo].[MD_Insert_ProyectoFormalizar]
	@CodProyecto int,
	@CONST_Inscripcion int, 
	@CodUsuario int, 
	@CONST_PlanAprobado int,
	@AVAL varchar(500),
	@Observacione varchar(500),
	@CodConvocatoriaFormal int,
	@CONST_Convocatoria int
AS

BEGIN	
	Declare @idConteo int
	SELECT @idConteo=count(*) FROM proyecto 
	WHERE codestado=@CONST_Inscripcion and id_Proyecto=@CodProyecto
	IF @idConteo > 0
	begin
		INSERT INTO Proyectoformalizacion (codProyecto, codContacto, Fecha, Aval, Observaciones, CodConvocatoria) 
		VALUES(@CodProyecto, @CodUsuario, getDate(),@AVAL,@Observacione,null)

		UPDATE Proyecto SET codEstado=@CONST_PlanAprobado WHERE id_Proyecto=@CodProyecto

		Select * from ConvocatoriaProyecto where codConvocatoria = @CodConvocatoriaFormal and codProyecto = @CodProyecto
		IF(@@ROWCOUNT = 0)
		Begin
			INSERT INTO ConvocatoriaProyecto (codConvocatoria, codProyecto, Fecha)
			VALUES(@CodConvocatoriaFormal,@CodProyecto, getDate())
		End

		UPDATE Proyecto SET codEstado=@CONST_Convocatoria WHERE id_Proyecto=@CodProyecto

		UPDATE ProyectoFormalizacion set codconvocatoria=@CodConvocatoriaFormal
		where codproyecto=@CodProyecto and codconvocatoria is null

		Select * from evaluacionproyectoindicador where codproyecto = @CodProyecto and codconvocatoria = @CodConvocatoriaFormal
		IF(@@ROWCOUNT = 0)
		Begin
			insert into evaluacionproyectoindicador (codproyecto,codconvocatoria,descripcion,tipo,valor,protegido)
			(select @CodProyecto, @CodConvocatoriaFormal, descripcion,tipo,0,1 from indicadormodelo)
		End
		

		Select * from evaluacionproyectosupuesto where codproyecto = @CodProyecto and codconvocatoria = @CodConvocatoriaFormal
		IF(@@ROWCOUNT = 0)
		Begin
			insert into evaluacionproyectosupuesto (nomevaluacionProyectosupuesto,codtiposupuesto,codproyecto,codconvocatoria)
			(select nomevaluacionproyectosupuesto,codtiposupuesto,@codProyecto, @CodConvocatoriaFormal
			from evaluacionproyectosupuestomodelo where inactivo=0)
		END

		Select * from EvaluacionIndicadorFinancieroProyecto where codproyecto = @CodProyecto and codconvocatoria = @CodConvocatoriaFormal
		IF(@@ROWCOUNT = 0)
		Begin
			insert into EvaluacionIndicadorFinancieroProyecto (descripcion,codproyecto,codconvocatoria)
			(select descripcion, @codproyecto, @CodConvocatoriaFormal
			from EvaluacionIndicadorFinancieromodelo where inactivo=0)
		END
		
		Select * from evaluacionrubroproyecto where codproyecto = @CodProyecto and codconvocatoria = @CodConvocatoriaFormal
		IF(@@ROWCOUNT = 0)
		Begin
			insert into evaluacionrubroproyecto (descripcion,codproyecto,codconvocatoria)
			(select descripcion, @codproyecto, @CodConvocatoriaFormal
			from evaluacionrubromodelo where inactivo=0)
		END
		
		Select * from evaluacionevaluador where codproyecto = @CodProyecto and codconvocatoria = @CodConvocatoriaFormal 
		IF(@@ROWCOUNT = 0)
		Begin
			insert into evaluacionevaluador (codproyecto, codconvocatoria, coditem)
			(select  @codproyecto , @CodConvocatoriaFormal, id_item from item)
		END
		
	end
	

END