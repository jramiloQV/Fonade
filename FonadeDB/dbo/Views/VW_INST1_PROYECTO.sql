﻿CREATE VIEW VW_INST1_PROYECTO (NomCiudad, NomInstitucion, NomUnidad, Id_Proyecto, NomProyecto) AS SELECT T1.NomCiudad, T2.NomInstitucion, T2.NomUnidad, T3.Id_Proyecto, T3.NomProyecto FROM Ciudad T1, Institucion T2, Proyecto T3 WHERE T1.Id_Ciudad=T2.CodCiudad AND T2.Id_Institucion=T3.CodInstitucion