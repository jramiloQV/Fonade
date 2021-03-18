﻿CREATE VIEW VW_PROYECTS_VIABLES_X_UNIDAD_EMPRENDIMIENTO (NomInstitucion, NomUnidad, NoProyectos_Viables) AS SELECT T2.NomInstitucion, T2.NomUnidad, Count(T3.Id_Proyecto) FROM ConvocatoriaProyecto T1, Institucion T2, Proyecto T3 WHERE T1.CodProyecto=T3.Id_Proyecto AND T2.Id_Institucion=T3.CodInstitucion AND T1.Viable=1 GROUP BY T2.NomInstitucion, T2.NomUnidad