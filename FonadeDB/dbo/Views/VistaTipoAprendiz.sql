CREATE  VIEW [dbo].[VistaTipoAprendiz] AS
select Id_Proyecto,NomProyecto,NomCiudad,NomDepartamento,NomInstitucion,NomUnidad,ValorRecomendado,NomSector,NomSubsector,Nombres,Apellidos,Identificacion,Genero,FechaNacimiento,Email,c.Telefono,NomTipoAprendiz,pf.Fecha,Nomestado,Justificacion,TituloObtenido,pf.CodConvocatoria
from contacto c
inner join proyectocontacto pc on c.id_contacto=pc.codcontacto
inner join proyecto p on p.id_proyecto=pc.codproyecto
inner join ciudad ciu on ciu.id_ciudad=p.codciudad
inner join departamento dep on dep.id_departamento=ciu.coddepartamento
inner join institucion i on i.id_institucion=p.codinstitucion
inner join subsector su on su.id_subsector=p.codsubsector
inner join sector on id_sector= su.codsector
left join tipoaprendiz on id_tipoaprendiz=c.codtipoaprendiz
inner join estado on id_estado=codestado
left join contactoestudio ce on id_contacto=ce.codcontacto
left join evaluacionobservacion eo on id_proyecto=eo.codproyecto
left join convocatoriaproyecto cp on id_proyecto=cp.codproyecto and eo.codconvocatoria=cp.codconvocatoria
left join proyectoFormalizacion pf on id_proyecto=pf.codproyecto and eo.codconvocatoria=pf.codconvocatoria
where codrol=3 and fechafin is null