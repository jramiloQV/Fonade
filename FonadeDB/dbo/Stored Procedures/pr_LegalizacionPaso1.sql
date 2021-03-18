CREATE PROCEDURE [dbo].[pr_LegalizacionPaso1]
   @codproyecto int 
AS
BEGIN  

 -- declaramos las variables
  declare @actividad as int
  declare @nomact as varchar(150)
  declare @codproy as int
  declare @item as int
  declare @metas as varchar(1000)
  declare @identifica as int
  declare @mes as int
  declare @codtipo as int
  declare @valor as money
  
  -- declaramos un cursor llamado "CursorPO".
  --set @codproyecto = 6166
 
  declare CursorPO cursor for
  select ID_Actividad, NomActividad, CodProyecto, Item, Metas from ProyectoActividadPO WHERE CodProyecto = @CodProyecto
  open CursorPO
  
  -- Avanzamos un registro y cargamos en las variables los valores encontrados en el primer registro
  fetch next from CursorPO
  into @actividad, @nomact, @codproy, @item, @metas
      while @@fetch_status = 0
		begin
		INSERT INTO ProyectoActividadPOInterventor (NomActividad,CodProyecto,Item,Metas) VALUES (Replace(@nomact,'''',''), @codproy, @item, Replace(@metas,'''',''))
		--insert into LogLegalizacion (Sentencia,parametro1,parametro2,parametro3,parametro4,fecha) values ('INSERT INTO ProyectoActividadPOInterventor (NomActividad,CodProyecto,Item,Metas) VALUES (',@nomact, @codproy, @item, @metas,getdate())
		-- Se hace la consulta para obtener identificador
		SELECT @identifica = id_actividad FROM ProyectoActividadPOInterventor WHERE CodProyecto = @CodProyecto AND NomActividad=Replace(@nomact,'''','')
		
		-- declaramos un cursor llamado "CursorPOMes".
		declare CursorPOMes cursor for
		SELECT  Mes, CodTipoFinanciacion, Valor FROM ProyectoActividadPOMes WHERE CodActividad = @actividad
		open CursorPOMes
        
		-- Avanzamos un registro y cargamos en las variables los valores encontrados en el primer registro
		fetch next from CursorPOMes
		into @mes, @codtipo, @valor
			while @@fetch_status = 0
				begin
				INSERT INTO ProyectoActividadPOMesInterventor (CodActividad,Mes,CodTipoFinanciacion,Valor) VALUES (@identifica, @mes, @codtipo, @valor)
			--insert into LogLegalizacion (Sentencia,parametro1,parametro2,parametro3,parametro4,fecha) values ('INSERT INTO ProyectoActividadPOMesInterventor (CodActividad,Mes,CodTipoFinanciacion,Valor) VALUES (',@identifica, @mes, @codtipo, @valor,getdate())
				-- Avanzamos otro registro
				fetch next from CursorPOMes
				into @mes, @codtipo, @valor
				end
				-- cerramos el cursor
		close CursorPOMes
		deallocate CursorPOMes
		
		--print  @identifica   

		-- Avanzamos otro registro
		fetch next from CursorPO
		into @actividad, @nomact, @codproy, @item, @metas
		end
    
	-- cerramos el cursor
	close CursorPO
	deallocate CursorPO

END