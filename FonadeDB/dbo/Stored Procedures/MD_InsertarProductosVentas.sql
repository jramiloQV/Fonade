-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_InsertarProductosVentas]
	-- Add the parameters for the stored procedure here
		@_CodProyecto int,
		@TotalRegistros int
AS
DECLARE @_Id_Producto int,
			@_NomProducto varchar(255),
			@_PorcentajeIva float,
			@_PorcentajeRetencion float,
			@_PorcentajeVentasContado float,
			@_PorcentajeVentasPlazo float,
			@_PosicionArancelaria char(10),
			@_PrecioLanzamiento money
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	    -- Insert statements for procedure here
		declare @producto uniqueidentifier , @date varchar(16),@TiempoProyeccion int, @Unidades int,@Precio varchar(10),  @Month int , @Years int, @Index int
		set @TiempoProyeccion =(SELECT TiempoProyeccion FROM [dbo].[ProyectoMercadoProyeccionVentas] WHERE CodProyecto = @_CodProyecto)
		
		SET @_PorcentajeIva=12
		SET @_PorcentajeRetencion=10
		SET @_PorcentajeVentasContado=15
		SET @_PosicionArancelaria ='1905000090'
		set @date=convert(varchar(16),getdate(),121)
		SET @_PorcentajeVentasPlazo =100-@_PorcentajeVentasContado
		SET @Index=1
		WHILE @Index <=@TotalRegistros
		BEGIN
			SET @_PrecioLanzamiento=156 * @Index
			set @producto =  newID()
	  		set @_NomProducto=@date+' ' + convert(varchar(50),@producto)
			IF(SELECT COUNT(1) FROM  ProyectoProducto WHERE NomProducto = @_NomProducto AND CodProyecto=@_CodProyecto )=0
			BEGIN
						INSERT INTO [ProyectoProducto]
							([CodProyecto],[NomProducto],[PorcentajeIva],[PorcentajeRetencion],[PorcentajeVentasContado],[PorcentajeVentasPlazo],[PosicionArancelaria],[PrecioLanzamiento])
						VALUES
							(@_CodProyecto,@_NomProducto,@_PorcentajeIva,@_PorcentajeRetencion,@_PorcentajeVentasContado,@_PorcentajeVentasPlazo,@_PosicionArancelaria,@_PrecioLanzamiento)
						set @_Id_Producto=(SELECT Id_Producto from ProyectoProducto WHERE NomProducto = @_NomProducto AND CodProyecto=@_CodProyecto )
						SET @Month=1
						while @Month<=12
						begin
							SET @Years=1
							SET @Unidades=23*@Month
							while @Years<=@TiempoProyeccion
							BEGIN
								INSERT	 INTO ProyectoProductoUnidadesVentas(CodProducto,Unidades,Mes,ano)values(@_Id_Producto,@Unidades,@Month,@Years)
								SET @Years=@Years+1
							END
							set @Month= @Month+1
						end 
						SET @Years=1
						while @Years<=@TiempoProyeccion
							BEGIN
							set	@Precio= convert(varchar(10),134*@Years)
								INSERT INTO ProyectoProductoPrecio(codproducto,Periodo, Precio)VALUES (@_Id_Producto, @Years,@Precio)
							SET @Years=@Years+1
							END
			END
			SET @Index=@Index+1
		END
		
--SELECT codproyecto,NomProyecto, @date Fecha_Hora_Grabacion, min(id_Producto) MinimoProducto ,max(id_Producto) MaximoProducto  ,@TotalRegistros TotalProductos, @TiempoProyeccion [Años de Proyección]
--FROM ProyectoProducto pp inner join proyecto p on p.id_proyecto=pp.codproyecto
--where codproyecto=@_CodProyecto and nomproducto like @date+'%'
--group by codproyecto,NomProyecto


--SELECT 'ProyectoProductoPrecio' Tabla ,@TotalRegistros TotalProductos, max(periodo) Iteraciones, max(periodo)*@TotalRegistros TotalRegistros  FROM ProyectoProducto pp 
--inner join proyecto p on p.id_proyecto=pp.codproyecto
--inner join ProyectoProductoPrecio pr on pr.CodProducto=pp.Id_Producto
--where codproyecto=@_CodProyecto and nomproducto like @date+'%'
--group by codproyecto,NomProyecto
--union all
--SELECT 'ProyectoProductoUnidadesVentas' Tabla , @TotalRegistros TotalProductos, MAX(ANO)* 12 Iteraciones , MAX(ANO)* 12 *@TotalRegistros TotalRegistros FROM ProyectoProducto pp 
--inner join proyecto p on p.id_proyecto=pp.codproyecto
--inner join ProyectoProductoUnidadesVentas pr on pr.CodProducto=pp.Id_Producto
--where codproyecto=@_CodProyecto and nomproducto like @date+'%'
--group by codproyecto,NomProyecto


/*		
select * from ProyectoProductoPrecio where CodProducto between 129004	and 129006
select * from ProyectoProductoUnidadesVentas where CodProducto between 129001 and	129003

exec MD_InsertarProductosVentas 59218,3
GO
SELECT top 15 p.id_proyecto, p.NomProyecto  FROM [ProyectoProducto] pp inner join Proyecto p on p.id_proyecto=pp.codproyecto
group by p.id_proyecto, p.NomProyecto  order by p.id_proyecto desc
where nomproducto like 'PRODUCTO%' ORDER BY Id_Producto DESC

SELECT TOP 5  CODPRODUCTO, SUM(UNIDADES) TOTAL, ANO FROM ProyectoProductoUnidadesVentas  
GROUP  BY  CODPRODUCTO, UNIDADES, ANO  ORDER BY CODProducto DESC

SELECT distinct TOP 30 codproyecto FROM [ProyectoProducto] order by codproyecto desc
select CodProducto , NomProducto, sum(unidades) totalUnidades, Ano from ProyectoProductoUnidadesVentas pv
inner join ProyectoProducto pp on pv.codproducto=pp.id_producto
where nomproducto like 'PRODUCTO TEST%' 
group by CodProducto,NomProducto,ano
order by CodProducto,NomProducto,ano
*/