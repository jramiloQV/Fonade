
CREATE PROCEDURE [dbo].[MD_ConceptoFinalRecomendacionesInterventor]
	
	@CodProyecto int,
	@Caso varchar(50),
	@CodActividad int
AS
BEGIN
	
	SET NOCOUNT ON;

    
	if @Caso = 'PLANOPERATIVO'
		begin
			Select DISTINCT a.CodActividad, a.Mes, convert(varchar,a.ObservacionesInterventor) as ObservacionesInterventor, p.NomActividad
			from AvanceActividadPOMes a, ProyectoActividadPOInterventor p
			where a.CodActividad = p.Id_Actividad and a.ObservacionesInterventor IS NOT NULL
			and p.CodProyecto = @CodProyecto
		end
	if @Caso = 'PLANOPERATIVO1'
		begin
			SELECT count(1)/2 as cuantos FROM AvanceActividadPOMes a, ProyectoActividadPOInterventor p
			WHERE a.CodActividad = @CodActividad AND a.CodActividad = p.Id_Actividad
			AND a.ObservacionesInterventor IS NOT NULL AND p.CodProyecto = @CodProyecto
		end
		
	if @Caso = 'NOMINA'
		begin
			SELECT DISTINCT a.CodCargo, a.Mes, convert(varchar,a.ObservacionesInterventor) as ObservacionesInterventor, i.Cargo as NomActividad
			FROM AvanceCargoPOMes a , InterventorNomina i
			WHERE a.CodCargo = i.Id_Nomina
			AND i.CodProyecto = @CodProyecto AND a.ObservacionesInterventor IS NOT NULL
		end
	if @Caso = 'NOMINA1'
		begin
			SELECT count(1)/2 as cuantos FROM AvanceCargoPOMes a, InterventorNomina i
			WHERE a.CodCargo = @CodActividad AND a.CodCargo = i.Id_Nomina
			AND i.CodProyecto = @CodProyecto
		end
		
	if @Caso = 'PRODUCCION'
		begin
			SELECT DISTINCT a.CodProducto, a.Mes, convert(varchar,a.ObservacionesInterventor) as ObservacionesInterventor, i.NomProducto as NomActividad
			FROM         AvanceProduccionPOMes a, InterventorProduccion i
			WHERE      a.CodProducto = i.id_produccion AND i.CodProyecto = @CodProyecto
			AND a.ObservacionesInterventor IS NOT NULL
		end
	if @Caso = 'PRODUCCION1'
		begin
			SELECT count(1)/2 as cuantos FROM AvanceProduccionPOMes a, InterventorProduccion i
			WHERE a.CodProducto = @CodActividad AND a.CodProducto = i.id_produccion
			AND a.ObservacionesInterventor IS NOT NULL AND i.CodProyecto = @CodProyecto
		end
		
	if @Caso = 'VENTAS'
		begin
			SELECT DISTINCT a.CodProducto, a.Mes, convert(varchar,a.ObservacionesInterventor) as ObservacionesInterventor, i.NomProducto as NomActividad
			FROM         AvanceVentasPOMes a, InterventorVentas i
			WHERE      a.CodProducto = i.id_ventas AND i.CodProyecto = @CodProyecto
			AND a.ObservacionesInterventor IS NOT NULL
		end
	if @Caso = 'VENTAS1'
		begin
			Select count(1)/2 as cuantos from AvanceVentasPOMes a, InterventorVentas i
			where a.CodProducto = @CodActividad and a.CodProducto = i.id_ventas
			and a.ObservacionesInterventor IS NOT NULL and i.CodProyecto = @CodProyecto
		end
		
	if @Caso = 'INDICADORESGENERICOS'
		begin
			SELECT NombreIndicador as NomActividad, Observacion as ObservacionesInterventor
			FROM IndicadorGenerico WHERE Observacion IS NOT NULL AND CodEmpresa =
			(SELECT Id_Empresa from Empresa where codproyecto = @CodProyecto)
		end
		
	if @Caso = 'INDICADORESESPECIFICOS'
		begin
			SELECT Aspecto as NomActividad, Observacion as ObservacionesInterventor
			FROM InterventorIndicador WHERE CodProyecto = @CodProyecto
			ORDER BY Id_IndicadorInter
		end
		
	if @Caso = 'RIESGOS'
		begin
			SELECT Riesgo as NomActividad, Observacion as ObservacionesInterventor
			FROM InterventorRiesgo WHERE CodProyecto = @CodProyecto
			ORDER BY Id_Riesgo
		end
END