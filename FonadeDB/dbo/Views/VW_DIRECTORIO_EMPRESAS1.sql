﻿CREATE VIEW VW_DIRECTORIO_EMPRESAS1 (NomCiudad, Nombres, Apellidos, CodConvocatoria, NomDepartamento, razonsocial, NomInstitucion, NomUnidad, Id_Proyecto, NomProyecto, Sumario, NomSector) AS SELECT T1.NomCiudad, T2.Nombres, T2.Apellidos, T3.CodConvocatoria, T4.NomDepartamento, T5.razonsocial, T6.NomInstitucion, T6.NomUnidad, T7.Id_Proyecto, T7.NomProyecto, T7.Sumario, T9.NomSector FROM Ciudad T1, Contacto T2, ConvocatoriaProyecto T3, departamento T4, Empresa T5, Institucion T6, Proyecto T7, ProyectoContacto T8, Sector T9, SubSector T10 WHERE T1.Id_Ciudad=T7.CodCiudad AND T2.Id_Contacto=T8.CodContacto AND T3.CodProyecto=T7.Id_Proyecto AND T4.Id_Departamento=T1.CodDepartamento AND T5.codproyecto=T7.Id_Proyecto AND T6.Id_Institucion=T7.CodInstitucion AND T8.CodProyecto=T7.Id_Proyecto AND T10.Id_SubSector=T7.CodSubSector AND T10.CodSector=T9.Id_Sector AND T8.CodRol=3 AND T7.CodEstado>=7