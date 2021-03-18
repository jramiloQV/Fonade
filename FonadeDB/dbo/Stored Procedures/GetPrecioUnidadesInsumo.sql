CREATE procedure [dbo].[GetPrecioUnidadesInsumo]
@CodInsumo numeric
as
 select  Mes, CONVERT(varchar, convert(money, isnull([1],0)), 1)  [Año1] ,
  CONVERT(varchar, convert(money, isnull([2],0)), 1)  [Año2],
  CONVERT(varchar, convert(money, isnull([3],0)), 1) [Año3],
  CONVERT(varchar, convert(money, isnull([4],0)), 1) [Año4],
  CONVERT(varchar, convert(money, isnull([5],0)), 1)[Año5],
   CONVERT(varchar, convert(money, isnull([6],0)), 1)  [Año6],
  CONVERT(varchar, convert(money, isnull([7],0)), 1) [Año7],
  CONVERT(varchar, convert(money, isnull([8],0)), 1) [Año8],
  CONVERT(varchar, convert(money, isnull([9],0)), 1)[Año9],
  CONVERT(varchar, convert(money, isnull([10],0)), 1)  [Año10],MesNum
 from(
 select ano,Mes MesNum,'Mes' +convert(varchar(2), mes) Mes,cantidad*  Unidades Unidades  from [dbo].[ProyectoProductoInsumo] pp , ProyectoInsumoPrecio pip ,[dbo].[ProyectoProductoUnidadesVentas] uv
 where pp.codInsumo=pip.codInsumo  and  uv.codproducto=pp.CodProducto and pp.codInsumo= @CodInsumo 
 group by ano,mes, cantidad,  Unidades 
)  p PIVOT( max(Unidades) for Ano IN([1],[2],[3],[4],[5],[6],[7],[8],[9],[10]) ) as pt 
union all
select 'Unidades' Mes, 
CONVERT(varchar, convert(money, isnull(sum([1]),0)), 1)  [1],
CONVERT(varchar, convert(money, isnull(sum([2]),0)), 1) [2],
CONVERT(varchar, convert(money, isnull(sum([3]),0)), 1) [3],
CONVERT(varchar, convert(money, isnull(sum([4]),0)), 1) [4],
CONVERT(varchar, convert(money, isnull(sum([5]),0)), 1) [5],
CONVERT(varchar, convert(money, isnull(sum([6]),0)), 1) [6] ,
CONVERT(varchar, convert(money, isnull(sum([7]),0)), 1) [7],
CONVERT(varchar, convert(money, isnull(sum([8]),0)), 1) [8],
CONVERT(varchar, convert(money, isnull(sum([9]),0)), 1) [9] ,
CONVERT(varchar, convert(money, isnull(sum([10]),0)), 1) [10] ,
13 MesNum from (
select pr.codInsumo, unidades,Precio,periodo  from [dbo].[ProyectoInsumoPrecio] pr inner join 
 (select codInsumo,ano, sum(cantidad*  Unidades)Unidades   from [dbo].[ProyectoProductoInsumo] pp ,[dbo].[ProyectoProductoUnidadesVentas] uv
 where uv.codproducto=pp.CodProducto and pp.codInsumo= @CodInsumo
 group by codInsumo, ano) un on pr.CodInsumo=un.CodInsumo and pr.periodo=un.ano
 where pr.codInsumo= @CodInsumo ) h pivot (max(unidades) for periodo in([1],[2],[3],[4],[5],[6],[7],[8],[9],[10])) m
union all 
 select 'Precio' Mes,
 CONVERT(varchar, convert(money, isnull(sum([1]),0)), 1)  [1],
CONVERT(varchar, convert(money, isnull(sum([2]),0)), 1) [2],
CONVERT(varchar, convert(money, isnull(sum([3]),0)), 1) [3],
CONVERT(varchar, convert(money, isnull(sum([4]),0)), 1) [4],
CONVERT(varchar, convert(money, isnull(sum([5]),0)), 1) [5],
CONVERT(varchar, convert(money, isnull(sum([6]),0)), 1) [6] ,
CONVERT(varchar, convert(money, isnull(sum([7]),0)), 1) [7],
CONVERT(varchar, convert(money, isnull(sum([8]),0)), 1) [8],
CONVERT(varchar, convert(money, isnull(sum([9]),0)), 1) [9] ,
CONVERT(varchar, convert(money, isnull(sum([10]),0)), 1) [10] ,
 14 MesNum from (
select pr.codInsumo, unidades*Precio Precio,periodo  from [dbo].[ProyectoInsumoPrecio] pr inner join 
 (select codInsumo,ano, sum(cantidad*  Unidades)Unidades   from [dbo].[ProyectoProductoInsumo] pp ,[dbo].[ProyectoProductoUnidadesVentas] uv
 where uv.codproducto=pp.CodProducto and pp.codInsumo= @CodInsumo -- and ano=1 
  group by codInsumo, ano) un on pr.CodInsumo=un.CodInsumo and pr.periodo=un.ano
 where pr.codInsumo= @CodInsumo ) h pivot (max(precio) for periodo in([1],[2],[3],[4],[5],[6],[7],[8],[9],[10])) m
 order by 12
 /*
 GetPrecioUnidadesInsumo  428509
 */