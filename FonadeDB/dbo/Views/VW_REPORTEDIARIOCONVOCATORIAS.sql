
CREATE VIEW [dbo].[VW_REPORTEDIARIOCONVOCATORIAS] (NomCiudad, Id_Convocatoria, NomConvocatoria, NomDepartamento, NomUnidad, Id_Proyecto, NomProyecto, Fecha) 
AS 

SELECT T1.NomCiudad, T2.Id_Convocatoria, T2.NomConvocatoria, T4.NomDepartamento, T5.NomUnidad, T6.Id_Proyecto, T6.NomProyecto, T7.Fecha FROM Ciudad T1, Convocatoria T2, ConvocatoriaProyecto T3, departamento T4, Institucion T5, Proyecto T6, ProyectoFormalizacion T7
where id_convocatoria=T3.codconvocatoria
and T6.id_proyecto = T3.codproyecto
and id_convocatoria=T7.codconvocatoria and id_proyecto=T7.codproyecto
and T1.id_ciudad=T6.codciudad and T4.id_departamento=coddepartamento
and T5.id_institucion=T6.codinstitucion and T2.Id_Convocatoria BETWEEN 316 AND 319