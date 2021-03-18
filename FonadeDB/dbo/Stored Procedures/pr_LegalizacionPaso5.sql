CREATE PROCEDURE [dbo].[pr_LegalizacionPaso5]
   @codproyecto int 
AS
BEGIN

-- declaramos las variables
  --declare @CodProyecto as int
  declare @Existe as int
  declare @nomProducto as varchar(100)
  declare @codProducto as int
  declare @idProducto as int
  declare @Unidades as int
  declare @Mes as int
  declare @Precio as money
  declare @identifica as int

  -- declaramos un cursor llamado "CursorVentas".
  --set @codproyecto = 6166
 
  declare CursorVentas cursor for
  SELECT Id_Producto, NomProducto FROM ProyectoProducto WHERE CodProyecto = @codproyecto ORDER BY Id_Producto
  open CursorVentas
  
  -- Avanzamos un registro y cargamos en las variables los valores encontrados en el primer registro
  fetch next from CursorVentas
  into @codProducto,@nomProducto
      while @@fetch_status = 0
		begin
		INSERT INTO InterventorVentas(CodProyecto,NomProducto) VALUES (@codproyecto, Left(Replace(@nomProducto,'''',''),100))
--insert into LogLegalizacion (Sentencia,parametro1,parametro2,fecha) values ('INSERT INTO InterventorVentas(CodProyecto,NomProducto) VALUES (',@codproyecto, Left(Replace(@nomProducto,'''',''),100),getdate())		
-- Se hace la consulta para obtener identificador
		set @Existe = isnull(
            (SELECT     count(id_Ventas)
            FROM  InterventorVentas
            where CodProyecto = @CodProyecto AND NomProducto=Left(Replace(@nomProducto,'''',''),100)),0)
		if (@Existe>0) begin
			SELECT top 1 @identifica = id_Ventas FROM InterventorVentas WHERE CodProyecto = @CodProyecto AND NomProducto=Left(Replace(@nomProducto,'''',''),100) ORDER BY id_Ventas DESC
		
			-- declaramos un cursor llamado "CursorProducto".
			declare CursorProducto cursor for
			SELECT     ProyectoProducto.Id_Producto, u.Unidades, u.Mes, p.Precio 
				FROM  ProyectoProducto LEFT OUTER JOIN ProyectoProductoUnidadesVentas AS u 
				ON ProyectoProducto.Id_Producto = u.CodProducto LEFT OUTER JOIN ProyectoProductoPrecio AS p 
				ON ProyectoProducto.Id_Producto = p.CodProducto WHERE     (p.Periodo = 1) 
				AND (ProyectoProducto.Id_Producto =  @codProducto) AND (u.Ano = 1) 
				AND (ProyectoProducto.CodProyecto = @CodProyecto) 
				ORDER BY ProyectoProducto.Id_Producto, u.Mes
			open CursorProducto
	        
			-- Avanzamos un registro y cargamos en las variables los valores encontrados en el primer registro
			fetch next from CursorProducto
			into @idProducto,@Unidades,@Mes,@Precio
				while @@fetch_status = 0
					begin
					if (Len(@identifica)>0) AND (Len(@Mes)>0) AND (Len(@Unidades)>0) begin
						INSERT INTO InterventorVentasMes (CodProducto,Mes,Valor,Tipo) VALUES (@identifica, @Mes, @Unidades, 1)
--insert into LogLegalizacion (Sentencia,parametro1,parametro2,parametro3,parametro4,fecha) values ('INSERT INTO InterventorVentasMes (CodProducto,Mes,Valor,Tipo) VALUES (',@identifica, @Mes, @Unidades, 1,getdate())		
--print  @identifica 					
end
					if (Len(@identifica)>0) AND (Len(@Mes)>0) AND (Len(@Precio)>0) begin
						INSERT INTO InterventorVentasMes (CodProducto,Mes,Valor,Tipo) VALUES (@identifica, @Mes, @Precio, 2)
--insert into LogLegalizacion (Sentencia,parametro1,parametro2,parametro3,parametro4,fecha) values ('INSERT INTO InterventorVentasMes (CodProducto,Mes,Valor,Tipo) VALUES (',@identifica, @Mes, @Precio, 2,getdate())		
--print  @identifica 					
end

					-- Avanzamos otro registro
					fetch next from CursorProducto
					into @idProducto,@Unidades,@Mes,@Precio
					end
					-- cerramos el cursor
			close CursorProducto
			deallocate CursorProducto

		end
		
		--print  @identifica   

		-- Avanzamos otro registro
		fetch next from CursorVentas
		into @codProducto,@nomProducto
		end
    
	-- cerramos el cursor
	close CursorVentas
	deallocate CursorVentas

END