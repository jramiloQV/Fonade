-- ============================================
-- Author:	 JOSE ANTONIO BOLAÑO OLIVEROS
-- Create date: 03/04/2014
-- Description:	Obtengo los items del proyecto seguimiento interventor
-- =============================================
CREATE PROCEDURE [dbo].[MD_ObtenerItems_Indicadores]
		@codproyecto INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
 

	SELECT	Id_IndicadorInter AS id_Actividad,
			Aspecto,
			FechaSeguimiento,
			numerador,
			Descripcion,
			RangoAceptable,
			Observacion,
			0 AS ActividadPo
	FROM InterventorIndicador 
	WHERE CodProyecto = @codproyecto
	
		--Select 
		--	poi.id_Actividad
		--	,poi.Item
		--	,poi.Nomactividad
		--	,poi.Metas
		--	,ISNULL((SELECT  TOP 1	CodActividad from AvanceActividadPOMes av WHERE av.CodActividad = poi.Id_Actividad),0) ActividadPo
		--From proyectoactividadPOInterventor poi
		--Where codproyecto = @codproyecto
		--Order by Item


		--Select id_Actividad,Item, Nomactividad, Metas
		--From proyectoactividadPOInterventorTMP
		--Where codproyecto = @codproyecto
	 --   Order by Item
	
END