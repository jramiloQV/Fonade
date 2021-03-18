create view ProyectoProyeccionesVentasTotal as
select sum(unidades) as TotalUnidades, sum(total) as TotalVentas, codproyecto 
from ProyectoProyeccionesVentas 
group by codproyecto, ano
having ano=1