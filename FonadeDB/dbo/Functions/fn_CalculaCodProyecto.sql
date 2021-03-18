CREATE FUNCTION dbo.fn_CalculaCodProyecto (@CodActividad int)
returns int
AS 
begin
  declare @puntos int
  
SELECT @puntos=ProyectoActividadPOInterventor.CodProyecto
FROM AvanceActividadPOAnexos 
INNER JOIN ProyectoActividadPOInterventor ON AvanceActividadPOAnexos.CodActividad = ProyectoActividadPOInterventor.Id_Actividad
WHERE (AvanceActividadPOAnexos.CodActividad = @CodActividad)

  return @puntos	
end