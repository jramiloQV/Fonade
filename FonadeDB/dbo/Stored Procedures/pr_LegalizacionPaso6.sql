CREATE PROCEDURE [dbo].[pr_LegalizacionPaso6]
   @codproyecto int 
AS
BEGIN  

 -- declaramos las variables
  declare @mitigacion as varchar(500)
  declare @riesgo as varchar(500)
  declare @aspecto as varchar(300)
  declare @fecha as varchar(60)
  declare @numerador as varchar(100)
  declare @denominador as varchar(100)
  declare @descripcion as varchar(300)
  declare @rango as int
    
  declare CursorPO cursor for
  select Riesgo, Mitigacion from EvaluacionRiesgo where codproyecto=@codproyecto
  open CursorPO
  
  -- Avanzamos un registro y cargamos en las variables los valores encontrados en el primer registro
  fetch next from CursorPO
  into @riesgo, @mitigacion
      while @@fetch_status = 0
		begin
		INSERT INTO InterventorRiesgo (CodProyecto,Riesgo,Mitigacion,CodEjeFuncional) VALUES (@codproyecto, Replace(@riesgo,'''',''), Replace(@mitigacion,'''',''), 2)
		 

		-- Avanzamos otro registro
		fetch next from CursorPO
		into @riesgo, @mitigacion
		end
    
	-- cerramos el cursor
	close CursorPO
	deallocate CursorPO

	declare CursorGestion cursor for
	  select Aspecto,FechaSeguimiento,Numerador,Denominador,Descripcion,RangoAceptable from EvaluacionIndicadorGestion where codproyecto=@codproyecto
	  open CursorGestion
	  
	  -- Avanzamos un registro y cargamos en las variables los valores encontrados en el primer registro
	  fetch next from CursorGestion
	  into @aspecto,@fecha,@numerador,@denominador,@descripcion,@rango
		  while @@fetch_status = 0
			begin
			INSERT INTO InterventorIndicador (CodProyecto,Aspecto,FechaSeguimiento,Numerador,Denominador,Descripcion,RangoAceptable,CodTipoIndicadorInter)
			VALUES (@codproyecto, Replace(@aspecto,'''',''), @fecha, Replace(@numerador,'''',''), Replace(@denominador,'''',''), Replace(@descripcion,'''',''), @rango,2)
			 

			-- Avanzamos otro registro
			fetch next from CursorGestion
			into @aspecto,@fecha,@numerador,@denominador,@descripcion,@rango
			end
	    
		-- cerramos el cursor
		close CursorGestion
		deallocate CursorGestion

END