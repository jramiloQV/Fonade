CREATE procedure [dbo].[GetDataPeriodosProductos](  @Codproducto numeric)
as

declare  @Index int, @IndexYears int, @TotalMonth int
declare  @PeriodosVentaFonade table(
	[Periodos] varchar(20) NULL,
	[Año 1] money NULL,
	[Año 2] money NULL,
	[Año 3] money NULL,
	[Año 4] money NULL,
	[Año 5] money NULL,
	[Año 6] money NULL,
	[Año 7] money NULL,
	[Año 8] money NULL,
	[Año 9] money NULL,
	[Año 10] money NULL,
	[Periodos_] varchar(20)
	)
declare	 @PeriodosPrecio table(
	CodProducto varchar(20) NULL,
	Periodo  tinyint NULL,
	Precio float null)

SET @Index=1
set @TotalMonth=12
insert into @PeriodosPrecio(CodProducto, Periodo,Precio)
		SELECT ppp.CodProducto,ppp.Periodo ,rtrim(replace(Precio,',','')) FROM [dbo].[ProyectoProductoPrecio] ppp 
		where CodProducto =@Codproducto

IF (select count(1) from ProyectoProducto PP INNER JOIN ProyectoProductoUnidadesVentas PV ON PV.CodProducto=PP.Id_Producto where Id_Producto=@Codproducto)=0
BEGIN
	WHILE  @Index <= @TotalMonth
	BEGIN    
		INSERT INTO @PeriodosVentaFonade([Año 1],	[Año 2] ,	[Año 3] ,	[Año 4] ,	[Año 5] ,	[Año 6] ,	[Año 7] ,	[Año 8] ,	[Año 9] ,	[Año 10] ,	[Periodos],	[Periodos_])
				values(0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	'Cant. Mes'+ convert(varchar(2),@Index) ,   	@Index  )
				set @IndexYears =@IndexYears + 1
		set @Index=@Index+1
		continue
	END
END
ELSE
	BEGIN
		INSERT INTO @PeriodosVentaFonade(Periodos,	[Año 1],	[Año 2] ,	[Año 3] ,	[Año 4] ,	[Año 5] ,	[Año 6] ,	[Año 7] ,	[Año 8] ,	[Año 9] ,	[Año 10] ,	[Periodos_])
		SELECT 'Cant. Mes'+ convert(varchar(20),Periodos) Periodos, [1],[2],[3],[4],[5],[6],[7],[8],[9],[10], convert(varchar(12),Periodos) Periodos_  
		FROM (SELECT CodProducto, Unidades, Mes Periodos,convert(varchar(12),Mes) Periodos_, Ano FROM [dbo].[ProyectoProductoUnidadesVentas] WHERE CodProducto = @Codproducto ) k 
		PIVOT(SUM(Unidades) FOR [Ano] IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10])) AS Unidades 
	END
IF (select count(1) from ProyectoProductoPrecio where Codproducto=@Codproducto)=0
	BEGIN
		INSERT INTO @PeriodosVentaFonade([Año 1],	[Año 2] ,	[Año 3] ,	[Año 4] ,	[Año 5] ,	[Año 6] ,	[Año 7] ,	[Año 8] ,	[Año 9] ,	[Año 10] ,	[Periodos],	[Periodos_])
		values(0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	'Precio'  ,   	'0'  )
	END
ELSE
	BEGIN
		INSERT INTO @PeriodosVentaFonade([Periodos],[Año 1],	[Año 2] ,	[Año 3] ,	[Año 4] ,	[Año 5] ,	[Año 6] ,	[Año 7] ,	[Año 8] ,	[Año 9] ,	[Año 10] ,[Periodos_])
		SELECT 'Precio'  Periodos, [1] [Año 1],[2] [Año 1],[3] [Año 3],[4] [Año 4],[5] [Año 5],[6] [Año 6],[7] [Año 7],[8] [Año 8],[9] [Año 9],[10] [Año 10],'Precio'   Periodos_ 
		FROM (SELECT CodProducto, Periodo ,Precio  FROM @PeriodosPrecio WHERE CodProducto = @Codproducto ) k 
		PIVOT(max(precio) FOR Periodo IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10])) AS Unidades 

	END
IF (select isnull(SUM(unidades),0) from ProyectoProductoUnidadesVentas where Codproducto=@Codproducto)=0
	BEGIN
		INSERT INTO @PeriodosVentaFonade([Año 1],	[Año 2] ,	[Año 3] ,	[Año 4] ,	[Año 5] ,	[Año 6] ,	[Año 7] ,	[Año 8] ,	[Año 9] ,	[Año 10] ,	[Periodos],	[Periodos_])
		Values(0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	0 ,   	'Ventas Esperadas'+ convert(varchar(2),@Index) ,   	'Ventas Esperadas'  )
	END
ELSE
	BEGIN
		INSERT INTO @PeriodosVentaFonade([Periodos],[Año 1],	[Año 2] ,	[Año 3] ,	[Año 4] ,	[Año 5] ,	[Año 6] ,	[Año 7] ,	[Año 8] ,	[Año 9] ,	[Año 10] ,[Periodos_])
		SELECT 'Ventas Esperadas'  Periodos, [1] [Año 1],[2] [Año 1],[3] [Año 3],[4] [Año 4],[5] [Año 5],[6] [Año 6],[7] [Año 7],[8] [Año 8],[9] [Año 9],[10] [Año 10],'Ventas Esperadas'   Periodos_  
		FROM (SELECT ppv.CodProducto,0 Periodos,ppv.Ano ,Precio*total total FROM  @PeriodosPrecio ppp inner join (
		select CodProducto,ano, sum(unidades)total  from ProyectoProductoUnidadesVentas where CodProducto =@Codproducto
		group by CodProducto, ano)  ppv on ppp.codproducto=ppv.CodProducto and ppp.Periodo=ppv.Ano
		WHERE ppv.CodProducto = @Codproducto ) k 
		PIVOT(max(total) FOR ano IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10])) AS Unidades 
	END
select Periodos, CONVERT(varchar, convert(money, isnull([Año 1],0)), 1) [Año 1],
		 CONVERT(varchar, convert(money, isnull([Año 2],0)), 1) [Año 2], CONVERT(varchar, convert(money, isnull([Año 3],0)), 1) [Año 3],
		 CONVERT(varchar, convert(money, isnull([Año 4],0)), 1) [Año 4], CONVERT(varchar, convert(money, isnull([Año 5],0)), 1) [Año 5],
		 CONVERT(varchar, convert(money, isnull([Año 6],0)), 1) [Año 6], CONVERT(varchar, convert(money, isnull([Año 7],0)), 1) [Año 7],
		 CONVERT(varchar, convert(money, isnull([Año 8],0)), 1) [Año 8], CONVERT(varchar, convert(money, isnull([Año 9],0)), 1) [Año 9],
		 CONVERT(varchar, convert(money, isnull([Año 10],0)), 1) [Año 10],Periodos_ from @PeriodosVentaFonade
/*
select CONVERT(varchar, convert(money, isnull([1],0)), 1) 
SELECT 'Precio'  Periodos, [1] [Año 1],[2] [Año 1],[3] [Año 3],[4] [Año 4],[5] [Año 5],[6] [Año 6],[7] [Año 7],[8] [Año 8],[9] [Año 9],[10] [Año 10],'Precio'   Periodos_ FROM (SELECT CodProducto, Periodo ,precio FROM [dbo].[ProyectoProductoPrecio] 
WHERE CodProducto = 106477) k 	PIVOT(max(precio) FOR Periodo IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10])) AS Unidades 

SELECT top 36 CodProducto, Unidades, Mes Periodos,convert(varchar(12),Mes) Periodos_, Ano FROM [dbo].[ProyectoProductoUnidadesVentas] --WHERE CodProducto = @Codproducto 
order by CodProducto desc
GetDataPeriodosProductos 295843
*/