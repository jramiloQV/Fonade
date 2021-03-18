CREATE    PROCEDURE [dbo].[pr_ProyectosAcreditadosExcel]
   @CodActa int, 
   @codConvocatoria int
AS
BEGIN
   
   select id_proyecto, nomproyecto, aap.Acreditado as viable, pa.CodEstado, i.NomUnidad
	, NomSubsector, NomSector, pa.Fecha, c.id_convocatoria, c.NomConvocatoria,Sumario
	FROM			AcreditacionActaProyecto AS aap INNER JOIN
                    AcreditacionActa AS aa ON aap.CodActa = aa.Id_Acta INNER JOIN
                    Proyecto AS p ON aap.CodProyecto = p.Id_Proyecto INNER JOIN
					Institucion i ON p.CodInstitucion = i.Id_Institucion INNER JOIN
					Subsector Sub ON p.CodSubsector = SUB.Id_Subsector INNER JOIN 
					Sector s ON SUB.CodSector = s.Id_Sector INNER JOIN 
					Convocatoria c ON (c.Id_Convocatoria=aa.codconvocatoria) INNER JOIN 
						(Select pa.* from 
					ProyectoAcreditacion pa
					inner join 
					(
					select max(id_proyectoAcreditacion) as id_proyectoAcreditacion
					from ProyectoAcreditacion 
					where CodConvocatoria = @codConvocatoria
					group by  CodProyecto, CodConvocatoria
					)TBL ON pa.Id_ProyectoAcreditacion = TBL.Id_ProyectoAcreditacion
					)pa ON pa.CodProyecto = aap.CodProyecto and pa.CodConvocatoria = aa.CodConvocatoria
                     
	WHERE     pa.CodEstado in (13,14)
		AND aap.CodActa = @CodActa
		AND aa.CodConvocatoria = @codConvocatoria
		
	order by id_proyecto, nomproyecto
					

END