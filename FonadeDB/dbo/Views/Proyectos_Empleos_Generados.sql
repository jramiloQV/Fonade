create view Proyectos_Empleos_Generados as 
select 
Id_Proyecto,NomProyecto,NomCiudad,Nomdepartamento,NomInstitucion,NomUnidad,pf.fecha,
nomsubsector,nomsector,pfi.recursos,eo.conclusionesfinancieras,eo.valorrecomendado,cp.viable,
p.sumario, nomevaluacionconceptos, eg.*
from empleosgenerados eg
inner join proyecto P on p.id_proyecto = eg.codproyecto
left join ciudad ciu on ciu.id_ciudad=p.codciudad
left join departamento dep on id_departamento =ciu.coddepartamento
left join institucion ins on id_institucion=p.codinstitucion
left join proyectoformalizacion pf on id_proyecto=pf.codproyecto
left join subsector sub on id_subsector=p.codsubsector
left join sector se on id_sector=sub.codsector
left join proyectofinanzasingresos pfi on p.id_proyecto=pfi.codproyecto
left join evaluacionobservacion eo on id_proyecto=eo.codproyecto and pf.codconvocatoria=eo.codconvocatoria
left join convocatoriaproyecto cp on  id_proyecto=cp.codproyecto and pf.codconvocatoria=cp.codconvocatoria
left join evaluacionconceptos on id_evaluacionconceptos=cp.codevaluacionconceptos