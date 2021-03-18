-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ActividadesDuplicadas] 
	-- Add the parameters for the stored procedure here
	@IdProyecto as int	
AS
BEGIN	
	SET NOCOUNT ON;    
	select 
	actividad.Id_Actividad,
	actividad.NomActividad, 
	(select count( Distinct AvanceActividadPOMes.mes) from AvanceActividadPOMes where CodActividad = actividad.Id_Actividad) AS AvancePorActividad,
	actividad.item,
	actividad.Metas,
	actividad.CodProyecto
	from 
	ProyectoActividadPOInterventor actividad
	inner join ( SELECT NomActividad, COUNT(*) AS VecesDuplicada FROM ProyectoActividadPOInterventor where ProyectoActividadPOInterventor.codproyecto = @IdProyecto GROUP BY NomActividad HAVING COUNT(*) > 1) duplicidad on actividad.NomActividad = duplicidad.NomActividad
	where actividad.codproyecto = @IdProyecto
	ORDER BY 
		actividad.NomActividad desc,
		AvancePorActividad desc
END