-- =============================================
-- Author:	 Alberto Palencia Benedetti
-- Create date: 28/03/2014
-- Description:	Obtengo los items del proyecto seguimiento interventor
-- =============================================
CREATE PROCEDURE [dbo].[MD_ObtenerItems_Interventor]
		@codproyecto INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
 
		Select 
			poi.id_Actividad
			,poi.Item
			,poi.Nomactividad
			,poi.Metas
			,ISNULL((SELECT  TOP 1	CodActividad from AvanceActividadPOMes av WHERE av.CodActividad = poi.Id_Actividad),0) ActividadPo
		From proyectoactividadPOInterventor poi
		Where codproyecto = @codproyecto
		Order by Item


		--Select id_Actividad,Item, Nomactividad, Metas
		--From proyectoactividadPOInterventorTMP
		--Where codproyecto = @codproyecto
	 --   Order by Item

		select id_Actividad,Item, Nomactividad, Metas 
		from proyectoactividadPOInterventorTMP 
		where codproyecto = @codProyecto
		order by Item
	
END