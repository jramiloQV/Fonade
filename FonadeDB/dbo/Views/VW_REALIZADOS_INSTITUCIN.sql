﻿CREATE VIEW VW_REALIZADOS_INSTITUCIN (NomInstitucion, NomUnidad, Realizados) AS SELECT T1.NomInstitucion, T1.NomUnidad, Count(T4.CodTab) FROM Institucion T1, Proyecto T2, ProyectoContacto T3, TabProyecto T4 WHERE T1.Id_Institucion=T2.CodInstitucion AND T2.Id_Proyecto=T3.CodProyecto AND T2.Id_Proyecto=T4.CodProyecto AND T4.Realizado=1 GROUP BY T1.NomInstitucion, T1.NomUnidad