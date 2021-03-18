CREATE VIEW [dbo].[ConvocatoriaProyectoDtosGrales]
AS
select 
id_convocatoria,NomConvocatoria,year(conv.fechainicio)as AñoConvocatoria,id_proyecto,NomProyecto,Recursos as RecursosSolicitados,
CONVERT(varchar,Month(pf.fecha),2) +'/'+CONVERT(varchar,day(pf.fecha),2) +'/'+ CONVERT(varchar,year(pf.fecha),4) as FechaFormalizacion,
NomSector,NomSubsector,ValorRecomendado,cp.Viable,NomEstado,ciu.NomCiudad,NomDepartamento,DomicilioEmpresa,e.telefono as TelefonoEmpresa,e.email as EmailEmpresa
,NomInstitucion,
NomUnidad,Nomtipoidentificacion,c.Identificacion,c.Nombres,c.Apellidos,c.Direccion,c.Telefono,c.Email,NomTipoAprendiz
from Convocatoria conv
inner join Convocatoriaproyecto cp on id_convocatoria=cp.codconvocatoria
inner join proyecto p on id_proyecto = cp.codproyecto
left join proyectofinanzasingresos pfi on id_proyecto=pfi.codproyecto
inner join proyectoformalizacion pf on id_convocatoria=pf.codconvocatoria and id_proyecto=pf.codproyecto
inner join subsector sub on id_subsector=p.codsubsector
inner join sector on id_sector=codsector
left join evaluacionobservacion eo on id_convocatoria=eo.codconvocatoria and id_proyecto=eo.codproyecto
inner join estado on id_estado=codestado
inner join ciudad ciu on ciu.id_ciudad=p.codciudad
inner join departamento on id_departamento=coddepartamento
left join empresa e on id_proyecto=e.codproyecto
inner join institucion on id_institucion=p.codinstitucion
inner join proyectocontacto pc on id_proyecto=pc.codproyecto
inner join contacto c on id_contacto=pc.codcontacto
left join tipoaprendiz on id_tipoaprendiz=codtipoaprendiz
Inner join tipoidentificacion on id_tipoidentificacion=c.codtipoidentificacion
where codrol=3