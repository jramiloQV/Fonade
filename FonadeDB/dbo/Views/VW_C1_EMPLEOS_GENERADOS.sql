﻿CREATE VIEW VW_C1_EMPLEOS_GENERADOS (Id_Proyecto, NomProyecto, EmpleoDirecto, EmpleoPrimerAno, Empleo18a24, EmpleoDesplazados, EmpleoMadres, EmpleoMinorias, EmpleoRecluidos, EmpleoDesmovilizados, EmpleoDiscapacitados, EmpleoDesvinculados, EmpleoIndirecto) AS SELECT T2.Id_Proyecto, T2.NomProyecto, T3.EmpleoDirecto, T3.EmpleoPrimerAno, T3.Empleo18a24, T3.EmpleoDesplazados, T3.EmpleoMadres, T3.EmpleoMinorias, T3.EmpleoRecluidos, T3.EmpleoDesmovilizados, T3.EmpleoDiscapacitados, T3.EmpleoDesvinculados, T3.EmpleoIndirecto FROM ConvocatoriaProyecto T1, Proyecto T2, ProyectoMetaSocial T3 WHERE T1.CodProyecto=T2.Id_Proyecto AND T3.CodProyecto=T2.Id_Proyecto AND T2.CodEstado>=7 AND T1.CodConvocatoria=1