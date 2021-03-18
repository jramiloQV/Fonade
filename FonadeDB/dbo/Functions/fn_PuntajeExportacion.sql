

CREATE FUNCTION fn_PuntajeExportacion (@CodProyecto int)
returns float
BEGIN
  DECLARE @Exp bit,
	  @PA char(10),
	  @puntaje int,
	  @cont int
  
  set @puntaje = 0
  set @cont = 0 --Contador de productos

  DECLARE CR_PA CURSOR FOR
  select Exportaciones, p.posicionarancelaria from PAExpImp e, proyectoproducto p 
  where e.posicionarancelaria = p.posicionarancelaria and codproyecto=@codproyecto

  OPEN CR_PA
	
  FETCH NEXT FROM CR_PA
  INTO @Exp, @PA
  
  while @@FETCH_STATUS = 0
  BEGIN
    set @cont = @cont + 1	
    if @PA = '0'
      set @puntaje = @puntaje + 7 --Servicio
    else
      if @Exp = 1 
	set  @puntaje = @puntaje + 4 --Ya Exportado
      else
	set  @puntaje = @puntaje + 10 --Nuevo producto a exportar

    FETCH NEXT FROM CR_PA
    INTO @Exp, @PA
  End

  CLOSE CR_PA
  DEALLOCATE CR_PA

  if @puntaje = 0
  begin
    set @puntaje = 10 --Nuevo producto a exportar 
    set @cont = 1	 
  End

  return (@puntaje/@cont)
End