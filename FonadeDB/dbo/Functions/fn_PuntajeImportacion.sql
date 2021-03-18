CREATE FUNCTION fn_PuntajeImportacion (@CodProyecto int)
returns float
BEGIN
  DECLARE @Imp bit,
	  @PA char(10),
	  @puntaje int,
	  @cont int
  
  set @puntaje = 0
  set @cont = 0 --Contador de productos

  DECLARE CR_PA CURSOR FOR
  select Importaciones, p.posicionarancelaria from PAExpImp i, proyectoproducto p 
  where i.posicionarancelaria = p.posicionarancelaria and codproyecto=@codproyecto

  OPEN CR_PA
	
  FETCH NEXT FROM CR_PA
  INTO @Imp, @PA
  
  while @@FETCH_STATUS = 0
  BEGIN
    set @cont = @cont + 1	
    if @PA = '0'
      set @puntaje = @puntaje + 7 --Servicio
    else
      if @Imp = 1 
	set  @puntaje = @puntaje + 10 --Ya Importado
      else
	set  @puntaje = @puntaje + 2 --Nuevo producto

    FETCH NEXT FROM CR_PA
    INTO @Imp, @PA
  End

  CLOSE CR_PA
  DEALLOCATE CR_PA

  if @puntaje = 0
  begin
    set @puntaje = 2 --Nuevo producto
    set @cont = 1	 
  End

  return (@puntaje/@cont)
End