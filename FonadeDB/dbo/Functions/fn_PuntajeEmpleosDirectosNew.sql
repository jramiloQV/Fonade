CREATE   FUNCTION fn_PuntajeEmpleosDirectosNew (@CodProyecto int, @CodConvocatoria int, @EmpleoEstimado int, @Anio char(4))
returns float
AS 
BEGIN
  DECLARE @EmpleoDirecto int,
	  @Valor int,
	  @puntaje float,
	  @SMLV money
 
  set @puntaje = 0

  select @SMLV = convert(money,convert(varchar(10),texto)) from texto 
  where nomtexto = 'TXT_SMLV'+ @Anio
 
  DECLARE CR_Empleo CURSOR FOR

  select empleodirecto, valorrecomendado 
  from proyectometasocial p, evaluacionobservacion e 
  where p.codproyecto=e.codproyecto and e.codproyecto=@codproyecto and codconvocatoria=@codconvocatoria

  OPEN CR_Empleo
	
  FETCH NEXT FROM CR_Empleo
  INTO  @EmpleoDirecto, @Valor

  if @@FETCH_STATUS = 0
  BEGIN
	if @Valor <> 0 
	  set @puntaje = (@EmpleoDirecto*10)/((@EmpleoEstimado*(@Valor*@SMLV))/1000000)
  End

  CLOSE CR_Empleo
  DEALLOCATE CR_Empleo
  
  return @puntaje
End