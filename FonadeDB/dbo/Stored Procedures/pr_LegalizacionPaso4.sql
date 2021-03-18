CREATE PROCEDURE [dbo].[pr_LegalizacionPaso4]
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

  -- declaramos un cursor llamado "CursorProduccion".
  --set @codproyecto = 6166
 
  declare CursorProduccion cursor for
  SELECT Id_Producto, NomProducto FROM ProyectoProducto WHERE CodProyecto = @codproyecto ORDER BY Id_Producto
  open CursorProduccion
  
  -- Avanzamos un registro y cargamos en las variables los valores encontrados en el primer registro
  fetch next from CursorProduccion
  into @codProducto,@nomProducto
      while @@fetch_status = 0
		begin
		INSERT INTO InterventorProduccion (CodProyecto,NomProducto) VALUES (@codproyecto, Left(Replace(@nomProducto,'''',''),100))
		--insert into LogLegalizacion (Sentencia,parametro1,parametro2,fecha) values ('INSERT INTO InterventorProduccion (CodProyecto,NomProducto) VALUES (',@codproyecto, Left(Replace(@nomProducto,'''',''),100),getdate())
		-- Se hace la consulta para obtener identificador
		set @Existe = isnull(
            (SELECT     count(id_Produccion)
            FROM  InterventorProduccion
            where CodProyecto = @CodProyecto AND NomProducto=Left(Replace(@nomProducto,'''',''),100)),0)
		if (@Existe>0) begin
			SELECT top 1 @identifica = id_Produccion FROM InterventorProduccion WHERE CodProyecto = @CodProyecto AND NomProducto=Left(Replace(@nomProducto,'''',''),100) ORDER BY id_Produccion DESC
		
			-- declaramos un cursor llamado "CursorProducto".
			declare CursorProducto cursor for
			SELECT     p.Id_Producto, u.Unidades, u.Mes, ISNULL(SUM(i.Cantidad * ip.Precio), 0) AS precio 
				FROM ProyectoProducto AS p LEFT OUTER JOIN ProyectoProductoUnidadesVentas AS u 
				ON p.Id_Producto = u.CodProducto LEFT OUTER JOIN ProyectoProductoInsumo AS i RIGHT OUTER JOIN 
				ProyectoInsumoPrecio AS ip ON i.CodInsumo = ip.CodInsumo ON p.Id_Producto = i.CodProducto 
				WHERE     (ip.Periodo = 1) AND (u.Ano = 1) AND (p.CodProyecto = @CodProyecto) 
				AND (p.Id_Producto = @codProducto) GROUP BY p.Id_Producto, u.Unidades, u.Mes 
				ORDER BY p.Id_Producto, u.Mes
			open CursorProducto
	        
			-- Avanzamos un registro y cargamos en las variables los valores encontrados en el primer registro
			fetch next from CursorProducto
			into @idProducto,@Unidades,@Mes,@Precio
				while @@fetch_status = 0
					begin
					if (Len(@identifica)>0) AND (Len(@Mes)>0) AND (Len(@Unidades)>0) begin
						INSERT INTO InterventorProduccionMes (CodProducto,Mes,Valor,Tipo) VALUES (@identifica, @Mes, @Unidades, 1)
--insert into LogLegalizacion (Sentencia,parametro1,parametro2,parametro3,parametro4,fecha) values ('INSERT INTO InterventorProduccionMes (CodProducto,Mes,Valor,Tipo) VALUES (',@identifica, @Mes, @Unidades, 1,getdate())
--print @identifica					
end
					if (Len(@identifica)>0) AND (Len(@Mes)>0) AND (Len(@Precio)>0) begin
						INSERT INTO InterventorProduccionMes (CodProducto,Mes,Valor,Tipo) VALUES (@identifica, @Mes, @Precio, 2)
--insert into LogLegalizacion (Sentencia,parametro1,parametro2,parametro3,parametro4,fecha) values ('INSERT INTO InterventorProduccionMes (CodProducto,Mes,Valor,Tipo) VALUES (',@identifica, @Mes, @Precio, 2,getdate())
--print @identifica					
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
		fetch next from CursorProduccion
		into @codProducto,@nomProducto
		end
    
	-- cerramos el cursor
	close CursorProduccion
	deallocate CursorProduccion

END