﻿CREATE VIEW VW_SENA_PROYECTOSVIABLESXUNIDAD (NomInstitucion, NomUnidad, No_Proyectos) AS SELECT T1.NomInstitucion, T1.NomUnidad, Count(T2.Id_Proyecto) FROM Institucion T1, Proyecto T2 WHERE T1.Id_Institucion=T2.CodInstitucion AND T2.CodEstado>=7 GROUP BY T1.NomInstitucion, T1.NomUnidad