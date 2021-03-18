CREATE VIEW [dbo].[vw_f2]
AS
SELECT DISTINCT p.Id_Proyecto,
                em.Nit AS Empresas,
                ci.NomCiudad AS Ciudad,
                d.Id_Departamento,
                d.NomDepartamento AS Departamento,
                sec.NomSector AS Sector
FROM   dbo.Proyecto AS p, dbo.Estado AS e, dbo.ConvocatoriaProyecto AS cp, dbo.Convocatoria AS c, dbo.Empresa AS em, dbo.Ciudad AS ci, dbo.Departamento AS d, dbo.subsector AS ss, dbo.sector AS sec, dbo.proyectoaporte AS pa, dbo.proyectogastospersonal AS pgp, (SELECT   p.id_proyecto AS proyecto,
                                                                                                                                                                                                                                                                             max(cp.Fecha) AS fecha
                                                                                                                                                                                                                                                                    FROM     dbo.ConvocatoriaProyecto AS cp, dbo.proyecto AS p
                                                                                                                                                                                                                                                                    WHERE    cp.CodProyecto = p.Id_Proyecto
                                                                                                                                                                                                                                                                    GROUP BY p.id_proyecto) AS f
WHERE  p.CodEstado = e.Id_Estado
       AND p.Id_Proyecto = cp.CodProyecto
       AND p.CodSubSector = ss.Id_SubSector
       AND sec.Id_Sector = ss.CodSector
       AND cp.CodConvocatoria = c.Id_Convocatoria
       AND f.proyecto = p.Id_Proyecto
       AND f.fecha = cp.fecha
       AND cp.Viable = 1
       AND em.codproyecto = p.Id_Proyecto
       AND p.CodCiudad = ci.Id_Ciudad
       AND ci.CodDepartamento = d.Id_Departamento
       AND p.codEstado NOT IN (4, 11, 10)
       AND p.Id_Proyecto = pgp.CodProyecto;