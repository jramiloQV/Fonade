CREATE PROCEDURE [dbo].[pr_LegalizacionPaso3]
   @codproyecto int 
AS
BEGIN  

-- declaramos las variables
  --declare @CodProyecto as int
  declare @Existe as int
  declare @nomInsumo as varchar(100)
  declare @codInsumo as int
  declare @SueldoMes as money
  declare @Generado as int
  declare @Sueldo as int
  declare @identifica as int
  declare @Mes as int

  -- declaramos un cursor llamado "CursorInsumo".
  --set @codproyecto = 6166
 
  declare CursorInsumo cursor for
  SELECT Id_Insumo, nomInsumo FROM ProyectoInsumo WHERE CodProyecto = @CodProyecto AND codTipoInsumo=2
  open CursorInsumo
  
  -- Avanzamos un registro y cargamos en las variables los valores encontrados en el primer registro
  fetch next from CursorInsumo
  into @codInsumo,@nomInsumo
      while @@fetch_status = 0
		begin
		INSERT INTO InterventorNomina (CodProyecto,Cargo,Tipo) VALUES (@CodProyecto, Replace(@nomInsumo,'''',''), 'Insumo')
		--insert into LogLegalizacion (Sentencia,parametro1,parametro2,parametro3,fecha) values ('INSERT INTO InterventorNomina (CodProyecto,Cargo,Tipo) VALUES (',@CodProyecto, Replace(@nomInsumo,'''',''), 'Insumo',getdate())
		-- Se hace la consulta para obtener identificador
		SELECT top 1 @identifica = id_Nomina FROM InterventorNomina WHERE CodProyecto = @CodProyecto AND Tipo='Insumo' ORDER BY id_Nomina DESC
		set @Existe = isnull(
            (SELECT     count(CodManoObra)
            FROM  ProyectoEmpleoManoObra
            where CodManoObra = @codInsumo),0)
		
		if (@Existe>0) begin
			SELECT top 1 @SueldoMes = SueldoMes, @Generado = GeneradoPrimerAno FROM ProyectoEmpleoManoObra WHERE CodManoObra = @codInsumo
			If (@Generado=0) begin
				set @Sueldo=0
			end
			else begin
				set @Sueldo=@SueldoMes
			end
			set @Mes=@Generado
			while @Mes<13 begin
				INSERT INTO InterventorNominaMes (CodCargo,Mes,Valor,Tipo) VALUES (@identifica, @Mes, @Sueldo, 1)
				--insert into LogLegalizacion (Sentencia,parametro1,parametro2,parametro3,parametro4,fecha) values ('INSERT INTO InterventorNominaMes (CodCargo,Mes,Valor,Tipo) VALUES (',@identifica, @Mes, @Sueldo, 1,getdate())
				set @Mes = @Mes+1
			end
            
		end
		else begin
			--Si no existe en la tabla se almacenan en ceros
			set @Sueldo=0
			set @Mes=1
			while @Mes<13 begin
				INSERT INTO InterventorNominaMes (CodCargo,Mes,Valor,Tipo) VALUES (@identifica, @Mes, @Sueldo, 1)
--insert into LogLegalizacion (Sentencia,parametro1,parametro2,parametro3,parametro4,fecha) values ('INSERT INTO InterventorNominaMes (CodCargo,Mes,Valor,Tipo) VALUES (',@identifica, @Mes, @Sueldo, 1,getdate())
				set @Mes = @Mes+1
			end
		end

		
		
		--print  @identifica   

		-- Avanzamos otro registro
		fetch next from CursorInsumo
		into @codInsumo,@nomInsumo
		end
    
	-- cerramos el cursor
	close CursorInsumo
	deallocate CursorInsumo

END