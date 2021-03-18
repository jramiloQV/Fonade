

CREATE FUNCTION fn_PuntajeEmpleosVulnerables(@CodProyecto int)
returns float
AS 
BEGIN
  DECLARE @EmpleoVulner  int,
	  @EmpleoDirecto int,
	  @puntaje float

  DECLARE CR_Empleo CURSOR FOR

  select (empleodesplazados + empleomadres + empleominorias + empleorecluidos +
          empleodesmovilizados + empleodiscapacitados + empleodesvinculados)
  	  as EmpleoVulner, Empleodirecto
  from proyectometasocial
  where codproyecto=@codproyecto
	
  OPEN CR_Empleo
	
  FETCH NEXT FROM CR_Empleo
  INTO @EmpleoVulner, @EmpleoDirecto

  if @@FETCH_STATUS = 0
  BEGIN
    if @EmpleoVulner <= @EmpleoDirecto and @EmpleoDirecto>0
	set @puntaje = @EmpleoVulner*10/ @EmpleoDirecto
     else
	set @puntaje = 0
  End

  CLOSE CR_Empleo
  DEALLOCATE CR_Empleo

  return @puntaje
End