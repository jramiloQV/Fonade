﻿CREATE VIEW VW_ICONO_PROYECTOS_CONVOCATORIA32 (NomCiudad, Fecha, NomDepartamento, NomInstitucion, NomUnidad, Id_Proyecto, NomProyecto, NomSector, NomSubSector) AS SELECT T1.NomCiudad, T2.Fecha, T3.NomDepartamento, T4.NomInstitucion, T4.NomUnidad, T5.Id_Proyecto, T5.NomProyecto, T6.NomSector, T7.NomSubSector FROM Ciudad T1, ConvocatoriaProyecto T2, departamento T3, Institucion T4, Proyecto T5, Sector T6, SubSector T7 WHERE T5.Id_Proyecto=T2.CodProyecto AND T2.CodConvocatoria=32 AND T5.CodInstitucion=T4.Id_Institucion AND T5.CodSubSector=T7.Id_SubSector AND T7.CodSector=T6.Id_Sector AND T5.CodCiudad=T1.Id_Ciudad AND T1.CodDepartamento=T3.Id_Departamento