create FUNCTION [dbo].[fn_CalculaCodProyectoVentas] (@CodActividad int)
returns int
AS 
begin
  declare @puntos int
  
select @puntos=Codproyecto from InterventorVentas where id_ventas=@CodActividad

  return @puntos	
end