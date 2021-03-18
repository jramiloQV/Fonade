create    FUNCTION fn_EmpleosPrimerAno (@CodProyecto int)
returns int
AS 
begin
declare @Empleos int
select @Empleos = count(EmpleoPrimerAno) from
	(select GeneradoPrimerAno as EmpleoPrimerAno
	from proyectogastospersonal m 
	inner join ProyectoEmpleoCargo ec on m.id_cargo=ec.codcargo
	where m.codproyecto=@CodProyecto 
	union
	select GeneradoPrimerAno as EmpleoPrimerAno
	from proyectoinsumo i 
	inner join ProyectoEmpleoManoObra em on i.id_insumo=em.codmanoobra
	where i.codproyecto=@CodProyecto) empleo 
where EmpleoPrimerAno>0

return @Empleos
end