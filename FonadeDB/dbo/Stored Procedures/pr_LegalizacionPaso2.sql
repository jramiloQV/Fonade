CREATE PROCEDURE [dbo].[pr_LegalizacionPaso2]
   @codproyecto int 
AS
BEGIN  

-- declaramos las variables
  declare @Existe as int
  declare @Generado as int
  declare @cargo as varchar(100)
  declare @Codcargo as int
  declare @valormensual as money
  declare @otrosgastos as money
  declare @identifica as int
  declare @Sueldo as int
  declare @Prestaciones as int
  declare @Mes as int
  
  -- declaramos un cursor llamado "CursorNomina".
  --set @codproyecto = 6166
 
  declare CursorNomina cursor for
  SELECT Id_Cargo, Cargo, ValorMensual, OtrosGastos FROM ProyectoGastosPersonal WHERE CodProyecto = @CodProyecto
  open CursorNomina
  
  -- Avanzamos un registro y cargamos en las variables los valores encontrados en el primer registro
  fetch next from CursorNomina
  into @Codcargo,@cargo,@valormensual,@otrosgastos
      while @@fetch_status = 0
		begin
		INSERT INTO InterventorNomina (CodProyecto,Cargo,Tipo) VALUES (@CodProyecto, Replace(@cargo,'''',''), 'Cargo')
		--insert into LogLegalizacion (Sentencia,parametro1,parametro2,parametro3,fecha) values ('INSERT INTO InterventorNomina (CodProyecto,Cargo,Tipo) VALUES (',@CodProyecto, Replace(@cargo,'''',''), 'Cargo',getdate())
		-- Se hace la consulta para obtener identificador
		SELECT top 1 @identifica = id_Nomina FROM InterventorNomina WHERE CodProyecto = @CodProyecto AND Tipo='Cargo' ORDER BY id_Nomina DESC
		
		set @Existe = isnull(
            (SELECT     count(CodCargo)
            FROM  proyectoempleocargo
            where CodCargo = @Codcargo),0)
		
		if (@Existe>0) begin
			SELECT top 1 @Generado = GeneradoPrimerAno FROM proyectoempleocargo WHERE CodCargo = @Codcargo
			if (@Generado=0) begin
				set @Sueldo=0
				set @Prestaciones=0
			end
			else begin
				set @Mes=13-@Generado
				set @Sueldo=@valormensual
				if @Mes = 0 begin
					set @Prestaciones=0
				end
				else begin
					set @Prestaciones=@otrosgastos/@Mes
				end
			end
			--Insertar el valor del sueldo
			set @Mes = @Generado
			while @Mes<13 begin
				INSERT INTO InterventorNominaMes (CodCargo,Mes,Valor,Tipo) VALUES (@identifica, @Mes, @Sueldo,1)
				--insert into LogLegalizacion (Sentencia,parametro1,parametro2,parametro3,parametro4,fecha) values ('INSERT INTO InterventorNominaMes (CodCargo,Mes,Valor,Tipo) VALUES (',@identifica, @Mes, @Sueldo,1,getdate())
				--print @Mes
				--print @Sueldo
				set @Mes = @Mes+1			
			end

			if (@Prestaciones<>0) begin
			--Insertar el valor de las prestaciones
				set @Mes = @Generado
				while @Mes<13 begin
					INSERT INTO InterventorNominaMes (CodCargo,Mes,Valor,Tipo) VALUES (@identifica, @Mes,@Prestaciones,2)
--insert into LogLegalizacion (Sentencia,parametro1,parametro2,parametro3,parametro4,fecha) values ('INSERT INTO InterventorNominaMes (CodCargo,Mes,Valor,Tipo) VALUES (',@identifica, @Mes,@Prestaciones,2,getdate())
					--print @Mes
					--print @Prestaciones
					set @Mes = @Mes+1				
				end
			end
            
		end
		else begin
			--Si no existe en la tabla se almacenan en ceros
			set @Sueldo=0
			set @Prestaciones=0
			set @Mes = 1
			while @Mes<13 begin
				INSERT INTO InterventorNominaMes (CodCargo,Mes,Valor,Tipo) VALUES (@identifica, @Mes, @Sueldo, 1)
--insert into LogLegalizacion (Sentencia,parametro1,parametro2,parametro3,parametro4,fecha) values ('INSERT INTO InterventorNominaMes (CodCargo,Mes,Valor,Tipo) VALUES (',@identifica, @Mes, @Sueldo, 1,getdate())
--print @Mes
					--print @Sueldo
				set @Mes = @Mes+1
			end
				set @Mes = 1
			while @Mes<13 begin
				INSERT INTO InterventorNominaMes (CodCargo,Mes,Valor,Tipo) VALUES (@identifica, @Mes, @Prestaciones,2)
--insert into LogLegalizacion (Sentencia,parametro1,parametro2,parametro3,parametro4,fecha) values ('INSERT INTO InterventorNominaMes (CodCargo,Mes,Valor,Tipo) VALUES (',@identifica, @Mes, @Prestaciones,2,getdate())
--print @Mes
					--print @Prestaciones
				set @Mes = @Mes+1			
			end
		end

		
		
		--print  @identifica   

		-- Avanzamos otro registro
		fetch next from CursorNomina
		into @Codcargo,@cargo,@valormensual,@otrosgastos
		end
    
	-- cerramos el cursor
	close CursorNomina
	deallocate CursorNomina

END