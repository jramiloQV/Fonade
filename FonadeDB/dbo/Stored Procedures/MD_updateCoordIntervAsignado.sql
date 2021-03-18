
CREATE PROCEDURE [dbo].[MD_updateCoordIntervAsignado]
	@codinterv int,
	@CodCoordInterv int
AS

BEGIN

	declare @comparaCoordInterv int

	select @comparaCoordInterv=isnull(codcoordinador,0) from interventor where codcontacto=@codinterv

	if @comparaCoordInterv != @CodCoordInterv

	begin 
	
		update Interventor set codcoordinador = @CodCoordInterv
		where codcontacto = @codinterv

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
			and codcontacto=@codinterv
			and codrol= 
			(
				select Id_Rol 
				from Rol 
				where Rol.Abreviacion='CIN'
			)
		)

		insert into proyectocontacto (CodProyecto,codcontacto,codrol,fechainicio,Codconvocatoria)
		(
			SELECT CodProyecto, @CodCoordInterv, (select Id_Rol from Rol where Rol.Abreviacion='CEV'), getdate(),max(codConvocatoria)
			FROM ConvocatoriaProyecto CP 
			WHERE codProyecto in 
			(
				select codproyecto 
				from proyectocontacto 
				where inactivo=0 
				and codcontacto=@codInterv
				and codrol= 
				(
					select Id_Rol 
					from Rol 
					where Rol.Abreviacion='CIN'
				)
			)
			Group by CodProyecto
		)

	end

END