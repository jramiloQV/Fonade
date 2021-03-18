
CREATE FUNCTION fn_PuntajeSectores (@CodProyecto int)
returns int
AS 
begin
  declare @puntos int
  
  select @puntos = rango from subsector, proyecto 
  where id_subsector=codsubsector and id_proyecto=@CodProyecto
  
  return @puntos	
end