﻿CREATE VIEW VW_C2_PUNTAJES_CRITERIOS_EVALUACION (Campo, codProyecto, codConvocatoria, Puntaje, NomProyecto, CodEstado) AS SELECT T1.Campo, T2.codProyecto, T2.codConvocatoria, T2.Puntaje, T3.NomProyecto, T3.CodEstado FROM Campo T1, EvaluacionCampo T2, Proyecto T3 WHERE T1.id_Campo=T2.codCampo AND T2.codProyecto=T3.Id_Proyecto AND T3.CodEstado=4 AND T2.codConvocatoria=2