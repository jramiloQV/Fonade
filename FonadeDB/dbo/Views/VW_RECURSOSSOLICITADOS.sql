﻿CREATE VIEW VW_RECURSOSSOLICITADOS (Id_Proyecto, NomProyecto, Recursos) AS SELECT T1.Id_Proyecto, T1.NomProyecto, T2.Recursos FROM Proyecto T1, ProyectoFinanzasIngresos T2 WHERE T1.Id_Proyecto=T2.CodProyecto AND T1.CodEstado=3