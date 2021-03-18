

CREATE FUNCTION fn_PuntajeIDH(@CodProyecto int)
returns float
AS 
BEGIN
  DECLARE @IDHMayor float,
	  @IDHMenor float,
	  @IDHProyecto float,
	  @IDHProm float
	
  -- Mayor IDH
  Select @IDHMayor = max(IDH) from ciudad 

  -- Menor IDH
  Select @IDHMenor = min(IDH) from ciudad 

  -- IDH del municipio al que pertenece el proyecto
  select @IDHProyecto = IDH from ciudad,proyecto 
  where id_ciudad=codciudad and id_proyecto=@codproyecto
  
  set @IDHProm = (10/(ABS(@IDHMenor-@IDHMayor))) * @IDHProyecto

  return @IDHProm *10 / @IDHMayor
End