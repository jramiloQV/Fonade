CREATE FUNCTION dbo.fn_CalculaCodProyectoProduccion (@CodActividad int)
returns int
AS 
begin
  declare @puntos int
  
select @puntos=Codproyecto from InterventorProduccion where id_produccion=@CodActividad

  return @puntos	
end