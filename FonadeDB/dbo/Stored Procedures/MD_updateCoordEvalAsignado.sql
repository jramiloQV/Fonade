
CREATE PROCEDURE [dbo].[MD_updateCoordEvalAsignado]
	@codeval int,
	@CodCoordEval int
AS

BEGIN

	declare @comparaCoordEval int

	select @comparaCoordEval=isnull(codcoordinador,0) from evaluador where codcontacto=@codeval

	if @comparaCoordEval != @CodCoordEval

	begin 
	
		update evaluador set codcoordinador = @CodCoordEval
		where codcontacto = @codeval

		Update proyectocontacto set inactivo=1, fechafin=getdate() 
		where inactivo=0 
		and CodRol = 
		(
			select Id_Rol 
			from Rol 
			where Rol.Abreviacion='CEV'
		)
		and CodProyecto in 
		(
			select codproyecto 
			from proyectocontacto 
			where inactivo=0 
			and codcontacto=@codeval 
			and codrol= 
			(
				select Id_Rol 
				from Rol 
				where Rol.Abreviacion='EV'
			)
		)

		insert into proyectocontacto (CodProyecto,codcontacto,codrol,fechainicio,Codconvocatoria)
		(
			SELECT CodProyecto, @CodCoordEval, (select Id_Rol from Rol where Rol.Abreviacion='CEV'), getdate(),max(codConvocatoria)
			FROM ConvocatoriaProyecto CP 
			WHERE codProyecto in 
			(
				select codproyecto 
				from proyectocontacto 
				where inactivo=0 
				and codcontacto=@codeval 
				and codrol= 
				(
					select Id_Rol 
					from Rol 
					where Rol.Abreviacion='EV'
				)
			)
			Group by CodProyecto
		)

	end

END