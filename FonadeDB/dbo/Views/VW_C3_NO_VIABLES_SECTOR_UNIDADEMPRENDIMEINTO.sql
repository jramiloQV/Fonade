﻿CREATE VIEW VW_C3_NO_VIABLES_SECTOR_UNIDADEMPRENDIMEINTO (NomCiudad, NomDepartamento, NomInstitucion, NomUnidad, Id_Proyecto, NomProyecto, Recursos, NomSector) AS SELECT T1.NomCiudad, T3.NomDepartamento, T4.NomInstitucion, T4.NomUnidad, T5.Id_Proyecto, T5.NomProyecto, T6.Recursos, T7.NomSector FROM Ciudad T1, ConvocatoriaProyecto T2, departamento T3, Institucion T4, Proyecto T5, ProyectoFinanzasIngresos T6, Sector T7, SubSector T8 WHERE T1.Id_Ciudad=T5.CodCiudad AND T1.CodDepartamento=T3.Id_Departamento AND T2.CodProyecto=T5.Id_Proyecto AND T4.Id_Institucion=T5.CodInstitucion AND T6.CodProyecto=T5.Id_Proyecto AND T8.Id_SubSector=T5.CodSubSector AND T8.CodSector=T7.Id_Sector AND T2.CodConvocatoria=4 AND T2.Viable=0 AND T5.CodEstado=4