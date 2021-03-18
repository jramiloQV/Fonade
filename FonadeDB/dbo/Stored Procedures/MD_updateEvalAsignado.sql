
CREATE PROCEDURE [dbo].[MD_updateEvalAsignado]
	@CodProyecto int,
	@CodEvalNuevo int,
	@CodConvocatoria int
AS

BEGIN

	declare @EvalActual int
	Select @EvalActual=codcontacto from proyectocontacto 
	where inactivo=0 and CodProyecto= @CodProyecto
	and CodRol = 4

	IF @EvalActual IS NOT NULL

		begin 
		
			IF @EvalActual <> @CodEvalNuevo
				
				BEGIN
	
					Update proyectocontacto set inactivo=1, fechafin=getdate() 
					where inactivo=0 and CodProyecto= @CodProyecto
					and CodRol IN (4, 5)

					insert into proyectocontacto (codproyecto,codcontacto,codrol,fechainicio,codconvocatoria) 
					values (@CodProyecto, @CodEvalNuevo, 4 ,getdate(), @CodConvocatoria)

					declare @CoordinEval int
					select @CoordinEval=codcoordinador from evaluador where codcontacto=@CodEvalNuevo

					if @CoordinEval IS NOT null
						begin

							insert into proyectocontacto (codproyecto,codcontacto,codrol,fechainicio,codconvocatoria)
							values (@CodProyecto,@CoordinEval,5,getdate(),@CodConvocatoria)
							UPDATE [dbo].[Proyecto] SET [CodEstado] = 4 WHERE (Id_Proyecto = @CodProyecto) 
						end

				END
		
		end

	ELSE
		BEGIN

			insert into proyectocontacto (codproyecto,codcontacto,codrol,fechainicio,codconvocatoria) 
					values (@CodProyecto, @CodEvalNuevo, 4 ,getdate(), @CodConvocatoria)

					declare @CoordinEval2 int
					select @CoordinEval2=codcoordinador from evaluador where codcontacto=@CodEvalNuevo

					if @CoordinEval2 IS NOT null
						begin

							insert into proyectocontacto (codproyecto,codcontacto,codrol,fechainicio,codconvocatoria)
							values (@CodProyecto,@CoordinEval2,5,getdate(),@CodConvocatoria)
							UPDATE [dbo].[Proyecto] SET [CodEstado] = 4 WHERE (Id_Proyecto = @CodProyecto) 
						end

		END
	

END