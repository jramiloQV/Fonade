﻿CREATE VIEW VW_CONV4AVALADOS (NomCiudad, NomDepartamento, NomInstitucion, NomUnidad, Id_Proyecto, NomProyecto, Fecha, NomSector, NomSubSector) AS SELECT T1.NomCiudad, T3.NomDepartamento, T4.NomInstitucion, T4.NomUnidad, T5.Id_Proyecto, T5.NomProyecto, T6.Fecha, T7.NomSector, T8.NomSubSector FROM Ciudad T1, ConvocatoriaProyecto T2, departamento T3, Institucion T4, Proyecto T5, ProyectoFormalizacion T6, Sector T7, SubSector T8 WHERE T1.Id_Ciudad=T5.CodCiudad AND T2.CodProyecto=T5.Id_Proyecto AND T3.Id_Departamento=T1.CodDepartamento AND T4.Id_Institucion=T5.CodInstitucion AND T5.Id_Proyecto=T6.codProyecto AND T8.Id_SubSector=T5.CodSubSector AND T8.CodSector=T7.Id_Sector AND T2.CodConvocatoria=32 AND T6.CodConvocatoria=32