﻿CREATE VIEW VW_C2_SECTOR_UNIDAD (CodConvocatoria, Viable, NomDepartamento, NomUnidad, Id_Proyecto, NomProyecto, CodEstado, NomSector) AS SELECT T2.CodConvocatoria, T2.Viable, T3.NomDepartamento, T4.NomUnidad, T5.Id_Proyecto, T5.NomProyecto, T5.CodEstado, T6.NomSector FROM Ciudad T1, ConvocatoriaProyecto T2, departamento T3, Institucion T4, Proyecto T5, Sector T6, SubSector T7 WHERE T1.CodDepartamento=T3.Id_Departamento AND T1.Id_Ciudad=T5.CodCiudad AND T2.CodProyecto=T5.Id_Proyecto AND T5.CodSubSector=T7.Id_SubSector AND T2.CodConvocatoria=2 AND T6.Id_Sector=T7.CodSector AND T4.Id_Institucion=T5.CodInstitucion