CREATE FUNCTION dbo.fn_CalculaCodProyectoCargo (@CodCargo int)
returns int
AS 
begin
  declare @puntos int
  
SELECT     @puntos=CodProyecto
FROM         dbo.InterventorNomina
WHERE     (Id_Nomina = @CodCargo)

  return @puntos	
end