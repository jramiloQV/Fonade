CREATE    PROCEDURE [dbo].[pr_ProyectosEvaluados]
   @CodActa int, 
   @codConvocatoria int
AS
BEGIN
   DECLARE @codRolEvaluador int

    SET @codRolEvaluador = 4	--Rol Evaluador


	/*
	MODIFICADO POR: JUAN DOMÍNGUEZ
	FECHA: 28/02/2017
	Se quita condición de convocatoria pues la información que se requiere consultar es la del acta y la del último evaluador así este haya sido asignado en una convocatorio anterior,
	al no estar relacionada en la última convocatoria donde se piensa presentar nuevamente el proyecto el query no entregaba información
	*/
   select 
			p.id_proyecto, p.nomproyecto, 
			ep.viable, 
			ev.id_contacto as idevaluador, ev.nombres+' '+ev.apellidos as evaluador, 
			c.id_contacto as idcoordinador, c.nombres+' '+c.apellidos as coordinador,
			eo.valorrecomendado, 
			case when isnull(cp.viable, 0)=1 then 'SI' else 'NO' end  as viableEvaluador


	from proyecto p   
	inner join proyectocontacto pc on p.id_proyecto=pc.codproyecto
	inner join evaluacionactaproyecto ep on p.id_proyecto=ep.codproyecto
	inner join convocatoriaproyecto cp on p.id_proyecto=cp.codproyecto
	inner join contacto ev on ev.id_contacto=pc.codcontacto 
	inner join evaluador e on e.codcontacto=pc.codcontacto 
	inner join contacto c on  c.id_contacto=e.codcoordinador 
	inner join evaluacionobservacion eo on pc.codproyecto=eo.codproyecto 
	and eo.codconvocatoria=cp.codconvocatoria and eo.codconvocatoria=pc.codconvocatoria
	where 
		ep.codacta=@codActa 
		--and  pc.codconvocatoria=(select CodConvocatoria from EvaluacionActa where Id_Acta = @CodActa)  
		and pc.codrol = @codRolEvaluador 
		and isnull(pc.fechafin,getdate())= (
									select max(isnull(fechafin,getdate())) from proyectocontacto 
									where codproyecto=p.id_proyecto 
									--and codconvocatoria=(select CodConvocatoria from EvaluacionActa where Id_Acta = @CodActa) 
									and codrol = @codRolEvaluador)
		order by cp.viable,id_proyecto, nomproyecto,fechafin

/*COPIA 28/02/2017

DECLARE @codRolEvaluador int

    SET @codRolEvaluador = 4	--Rol Evaluador

   select id_proyecto, nomproyecto, ep.viable, ev.id_contacto as idevaluador, ev.nombres+' '+ev.apellidos as evaluador, 
	c.id_contacto as idcoordinador, c.nombres+' '+c.apellidos as coordinador,valorrecomendado, case when isnull(cp.viable, 0)=1 then 'SI' else 'NO' end  as viableEvaluador
	from proyectocontacto pc 
	inner join proyecto p on p.id_proyecto=pc.codproyecto
	inner join evaluacionactaproyecto ep on p.id_proyecto=ep.codproyecto
	inner join convocatoriaproyecto cp on p.id_proyecto=cp.codproyecto
	inner join contacto ev on ev.id_contacto=pc.codcontacto 
	inner join evaluador e on e.codcontacto=pc.codcontacto 
	inner join contacto c on  c.id_contacto=e.codcoordinador 
	inner join evaluacionobservacion eo on pc.codproyecto=eo.codproyecto 
	and eo.codconvocatoria=cp.codconvocatoria and eo.codconvocatoria=pc.codconvocatoria
	where 
		codacta=@codActa 
		and  pc.codconvocatoria=(select CodConvocatoria from EvaluacionActa where Id_Acta = @CodActa)  
		and codrol = @codRolEvaluador and isnull(fechafin,getdate())= (select max(isnull(fechafin,getdate())) from proyectocontacto 
		where codproyecto=p.id_proyecto and codconvocatoria=(select CodConvocatoria from EvaluacionActa where Id_Acta = @CodActa) 
		and codrol = @codRolEvaluador) order by cp.viable,id_proyecto, nomproyecto,fechafin
*/




END