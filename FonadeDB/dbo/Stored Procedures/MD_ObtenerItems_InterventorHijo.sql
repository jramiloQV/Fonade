-- =============================================
-- Author:	 Alberto Palencia Benedetti
-- Create date: 28/03/2014
-- Description:	Obtengo los items del proyecto seguimiento interventor HIJOS

-- [MD_ObtenerItems_InterventorHijo] 47876,87168
-- =============================================
CREATE PROCEDURE [dbo].[MD_ObtenerItems_InterventorHijo]
		@codproyecto INT
		,@actividad INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT
		DISTINCT 
		 ISNULL(pm.CodActividad,0) CodActividad
		,ISNULL(pm.Mes,0) Mes
		,ISNULL(pm.CodTipoFinanciacion,0) CodTipoFinanciacion
		,ISNULL(CONVERT(VARCHAR(2000),CAST(pm.Valor AS DECIMAL),1) ,0.0) Valor
		,pin.Id_Actividad
		,pin.NomActividad
		,pin.CodProyecto
		,pin.Item
		,pin.Metas
	FROM ProyectoActividadPOMesInterventor pm
	RIGHT OUTER JOIN proyectoactividadPOInterventor pin
	On pin.id_actividad= pm.CodActividad 
	Where pin.codproyecto= @codproyecto AND pin.Id_actividad = @actividad
	ORDER BY item, mes,codtipofinanciacion
	
END