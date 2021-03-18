CREATE VIEW v_temporal as 
SELECT c.NomConvocatoria, 
f.Fecha,
p.Id_Proyecto,
REPLACE(p.NomProyecto, CHAR(9), ' ') Proyecto,
ci.NomCiudad,
d.NomDepartamento,
valores.Solicitado,
valores.Recomendado,
evaluadores.id_cont,
evaluadores.nombre,
CASE cp.Viable WHEN 1 THEN 'SI' ELSE 'NO' END Viabilidad,
REPLACE(eo.conclusionesfinancieras,CHAR(13)+CHAR(10), ' ') observacion
FROM Convocatoria c,
proyecto p,
ConvocatoriaProyecto cp,
ProyectoFormalizacion f,
ciudad ci,
departamento d,
(select ea.CodProyecto id, 
sum(ea.Recomendado) Recomendado,
sum(ea.Solicitado) Solicitado
from EvaluacionProyectoAporte ea
group by ea.CodProyecto) valores,
(select distinct p.Id_Proyecto id_proy,co.Id_Contacto id_cont,co.Nombres + ' ' + co.Apellidos nombre
from proyecto p,
ProyectoContacto pc,
Contacto co,
GrupoContacto gc,
grupo g
WHERE pc.CodProyecto = p.Id_Proyecto
AND pc.CodContacto = co.Id_Contacto
AND gc.CodContacto = co.Id_Contacto
AND gc.CodGrupo = g.Id_Grupo
AND g.Id_Grupo = 11) evaluadores,
Evaluacionobservacion eo
WHERE cp.CodConvocatoria = c.Id_Convocatoria
AND cp.CodProyecto = p.Id_Proyecto
AND p.Id_Proyecto = f.codProyecto
AND p.CodCiudad = ci.Id_Ciudad
AND ci.CodDepartamento = d.Id_Departamento
AND valores.id = p.Id_Proyecto
AND evaluadores.id_proy = p.Id_Proyecto
AND eo.CodProyecto = p.Id_Proyecto