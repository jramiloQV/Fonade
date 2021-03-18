
CREATE PROCEDURE MD_InsertUpdateproyectoresumenejecutivo
	@codigoProyecto int,
	@txtConcepto text,
	@txtPotencial text,
	@txtVentajas text,
	@txtInversiones text,
	@txtProyecciones text,
	@txtConclusiones text

AS

BEGIN
	declare @conteo int
	select @conteo=count(*) from ProyectoResumenEjecutivo where codproyecto=@codigoProyecto

	IF @conteo=0

		begin
	
			insert into proyectoresumenejecutivo (CodProyecto,ConceptoNegocio,PotencialMercados,
				VentajasCompetitivas,ResumenInversiones,Proyecciones,ConclusionesFinancieras)
	
			values (@codigoProyecto, @txtConcepto, @txtPotencial, @txtVentajas, @txtInversiones, 
				@txtProyecciones, @txtConclusiones)

		end

	ELSE

		begin
	
			update proyectoresumenejecutivo set
			ConceptoNegocio = @txtConcepto,
			PotencialMercados = @txtPotencial,
			VentajasCompetitivas = @txtVentajas,
			ResumenInversiones = @txtInversiones,
			Proyecciones = @txtProyecciones,
			ConclusionesFinancieras = @txtConclusiones
			where codproyecto=@codigoProyecto

		end
END