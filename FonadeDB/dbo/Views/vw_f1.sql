CREATE VIEW vw_f1 AS
SELECT DISTINCT 
p.Id_Proyecto, 
em.Nit Empresas,
ci.NomCiudad Ciudad, 
d.Id_Departamento,
d.NomDepartamento Departamento,
sec.NomSector Sector,
pgp.Id_Cargo
FROM 
Proyecto p,
Estado e,
ConvocatoriaProyecto cp,
Convocatoria c,
Empresa em,
Ciudad ci,
Departamento d,
subsector ss,
sector sec,
proyectoaporte pa,
proyectogastospersonal pgp,
(	select p.id_proyecto proyecto, max(cp.Fecha) fecha
	FROM ConvocatoriaProyecto cp, proyecto p
	WHERE cp.CodProyecto = p.Id_Proyecto	
	group by p.id_proyecto) fecha
WHERE 
p.CodEstado = e.Id_Estado
AND p.Id_Proyecto = cp.CodProyecto
AND p.CodSubSector = ss.Id_SubSector
AND sec.Id_Sector = ss.CodSector
AND cp.CodConvocatoria = c.Id_Convocatoria
AND fecha.proyecto = p.Id_Proyecto
AND fecha.fecha = cp.fecha
AND cp.Viable = 1
AND em.codproyecto = p.Id_Proyecto
AND p.CodCiudad = ci.Id_Ciudad
AND ci.CodDepartamento = d.Id_Departamento
AND p.codEstado not in (4,11,10) 
AND p.Id_Proyecto = pgp.CodProyecto

--Cantidad de cargos
--SELECT SUM(COUNT(pgp.Id_Cargo)) Cantidad
--FROM vw_fase1 f,
--ProyectoGastosPersonal pgp
--WHERE f.id_proyecto = pgp.CodProyecto