CREATE VIEW vw_vista_total AS
SELECT DISTINCT 
p.Id_Proyecto, 
em.Nit Empresa,
ci.Id_Ciudad Ciudad, 
d.Id_Departamento,
sec.Id_Sector,
sec.NomSector Sector,
d.NomDepartamento,
eo.ValorRecomendado * sm.SalarioMinimo as recursos,
eg.Total Empleos,
c.Id_Convocatoria,
c.FechaInicio,
sm.SalarioMinimo,
eo.ValorRecomendado
FROM 
dbo.Proyecto p,
dbo.Estado e,
dbo.ConvocatoriaProyecto cp,
dbo.Convocatoria c,
dbo.Empresa em,
dbo.Ciudad ci,
dbo.Departamento d,
dbo.subsector ss,
dbo.Evaluacionobservacion eo,
dbo.sector sec,
dbo.proyectoaporte pa,
dbo.proyectogastospersonal pgp,
dbo.EmpleosGenerados eg,
dbo.SalariosMinimos sm,
(	select p.id_proyecto proyecto, max(cp.Fecha) fecha
	FROM dbo.ConvocatoriaProyecto cp, dbo.proyecto p
	WHERE cp.CodProyecto = p.Id_Proyecto	
	group by p.id_proyecto) as f
WHERE 
p.CodEstado = e.Id_Estado
AND sm.AñoSalario = (select (DatePart(yyyy,c.fechaInicio)))
AND p.Id_Proyecto = cp.CodProyecto
AND p.Id_Proyecto = eo.CodProyecto
AND p.Id_Proyecto = eg.CodProyecto
AND p.CodSubSector = ss.Id_SubSector
AND sec.Id_Sector = ss.CodSector
AND cp.CodConvocatoria = c.Id_Convocatoria
AND f.proyecto = p.Id_Proyecto
AND f.fecha = cp.fecha
AND cp.Viable = 1
AND em.codproyecto = p.Id_Proyecto
AND p.CodCiudad = ci.Id_Ciudad
AND ci.CodDepartamento = d.Id_Departamento
AND p.codEstado not in (4,11,10) 
AND p.Id_Proyecto = pgp.CodProyecto
AND c.Id_Convocatoria = eo.CodConvocatoria