create view VISTA_VENTAS_REGISTRADAS as
select codproyecto, sum(valor) as TotalVentas from avanceventaspomes,interventorventas
where codtipofinanciacion=2
and Id_ventas = codproducto
group by codproyecto