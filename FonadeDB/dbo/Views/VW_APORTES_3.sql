﻿CREATE VIEW VW_APORTES_3 (Nombre, Solicitado, Recomendado, Id_Proyecto, NomProyecto, CodEstado) AS SELECT T1.Nombre, T1.Solicitado, T1.Recomendado, T2.Id_Proyecto, T2.NomProyecto, T2.CodEstado FROM EvaluacionProyectoAporte T1, Proyecto T2 WHERE T2.Id_Proyecto=T1.CodProyecto