

CREATE FUNCTION fn_PuntajeEmpleosJoven (@CodProyecto int)
returns float
AS 
BEGIN
  DECLARE @EmpleoJoven int,
	  @EmpleoDirecto int,
	  @puntaje float

  DECLARE CR_EmpleoJoven CURSOR FOR

--  select Empleo18a24, Empleodirecto
--  from proyectometasocial
--  where codproyecto=@codproyecto

select (select count(Joven) from proyectoempleocargo where codcargo In(
select Id_cargo from proyectogastospersonal where codproyecto=@codproyecto) and joven=1)
+
(select count(joven) from proyectoempleomanoobra where codmanoobra in(
select id_Insumo from proyectoinsumo where codtipoinsumo=2 and codproyecto=@codproyecto) and joven=1) as Empleo18a24,
((select count(*) as EmpleosDirectos from proyectoinsumo where codtipoinsumo=2 and codproyecto=@codproyecto)
+
(select count(*) from proyectogastospersonal where codproyecto=@codproyecto)) as EmpleoDirecto

	
  OPEN CR_EmpleoJoven
	
  FETCH NEXT FROM CR_EmpleoJoven
  INTO @EmpleoJoven, @EmpleoDirecto

  if @@FETCH_STATUS = 0
  BEGIN
    if @EmpleoJoven <= @EmpleoDirecto and @EmpleoDirecto>0
	set @puntaje = @EmpleoJoven*10/ @EmpleoDirecto
     else
	set @puntaje = 0
  End

  CLOSE CR_EmpleoJoven
  DEALLOCATE CR_EmpleoJoven
  
  return @puntaje
End