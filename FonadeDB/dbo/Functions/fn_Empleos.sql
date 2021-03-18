CREATE  FUNCTION fn_Empleos ()
returns table
AS 
return
select sum(EmpleoDirecto) as empleodirecto, sum(Empleo18a24) as Empleo18a24, 
sum(EmpleoDesplazados) as EmpleoDesplazados, sum(EmpleoMadres) as EmpleoMadres, 
sum(EmpleoMinorias) as EmpleoMinorias, sum(EmpleoRecluidos) as EmpleoRecluidos, 
sum(EmpleoDesmovilizados) as EmpleoDesmovilizados, sum(EmpleoDiscapacitados) as EmpleoDiscapacitados, 
sum(EmpleoDesvinculados) as EmpleoDesvinculados, codproyecto
from
	(select count(ec.codcargo) as EmpleoDirecto, sum(cast(joven as int)) as Empleo18a24, sum(cast(desplazado as int)) as EmpleoDesplazados, sum(cast(madre as int)) as EmpleoMadres, 
	sum(cast(minoria as int)) as EmpleoMinorias, sum(cast(recluido as int)) as EmpleoRecluidos, sum(cast(desmovilizado as int)) as EmpleoDesmovilizados, 
	sum(cast(discapacitado as int)) as EmpleoDiscapacitados, sum(cast(desvinculado as int)) as EmpleoDesvinculados, codproyecto
	from proyectogastospersonal m 
	inner join ProyectoEmpleoCargo ec on m.id_cargo=ec.codcargo
	group by codproyecto
	union
	select count(em.codmanoobra) as EmpleoDirecto,sum(cast(joven as int)) as Empleo18a24, sum(cast(desplazado as int)) as EmpleoDesplazados, sum(cast(madre as int)) as EmpleoMadres, 
	sum(cast(minoria as int)) as EmpleoMinorias, sum(cast(recluido as int)) as EmpleoRecluidos, sum(cast(desmovilizado as int)) as EmpleoDesmovilizados, 
	sum(cast(discapacitado as int)) as EmpleoDiscapacitados, sum(cast(desvinculado as int)) as EmpleoDesvinculados , codproyecto
	from proyectoinsumo i 
	inner join ProyectoEmpleoManoObra em on i.id_insumo=em.codmanoobra
	group by codproyecto) empleos
group by codproyecto