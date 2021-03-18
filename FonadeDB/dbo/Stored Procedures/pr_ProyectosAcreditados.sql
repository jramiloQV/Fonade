CREATE    PROCEDURE [dbo].[pr_ProyectosAcreditados]
   @CodActa int, 
   @codConvocatoria int
AS
BEGIN
   
   select id_proyecto, nomproyecto, aap.Acreditado as viable, c.id_contacto as idAcreditador
	, c.nombres+' '+c.apellidos as Acreditador
	, isnull((select case when CodEstado=13 then 'SI' else 'NO' end  as ViableAcreditador 
	from ProyectoAcreditacion pa
	inner join
	(
	select max(id_proyectoAcreditacion) as id_proyectoAcreditacion, CodProyecto, CodConvocatoria 
	from ProyectoAcreditacion 
	where CodProyecto = p.Id_Proyecto and CodConvocatoria = aa.CodConvocatoria
	group by  CodProyecto, CodConvocatoria
	)TBL on tbl.id_proyectoAcreditacion = pa.id_proyectoAcreditacion 
	 where TBL.CodProyecto = p.Id_Proyecto and TBL.CodConvocatoria = aa.CodConvocatoria
	),'NO')
	as viableAcreditador
	FROM         AcreditacionActaProyecto AS aap INNER JOIN
                      AcreditacionActa AS aa ON aap.CodActa = aa.Id_Acta INNER JOIN
                      Proyecto AS p ON aap.CodProyecto = p.Id_Proyecto INNER JOIN
                      ProyectoContacto AS pc ON p.Id_Proyecto = pc.CodProyecto and pc.CodConvocatoria=@codConvocatoria INNER JOIN
                      Contacto AS c ON pc.CodContacto = c.Id_Contacto 
	WHERE     (aap.CodActa = @CodActa) 
	AND (pc.Acreditador = 1) 
	AND (pc.CodConvocatoria = aa.CodConvocatoria) 
	--AND (pc.Inactivo = 0) 
	AND (aa.CodConvocatoria = @codConvocatoria)
		
	order by id_proyecto, nomproyecto

END