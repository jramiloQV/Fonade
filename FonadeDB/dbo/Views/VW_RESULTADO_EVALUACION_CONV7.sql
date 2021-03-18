﻿CREATE VIEW VW_RESULTADO_EVALUACION_CONV7 (Id_Convocatoria, NomConvocatoria, Justificacion, Viable, Id_Proyecto, NomProyecto) AS SELECT T1.Id_Convocatoria, T1.NomConvocatoria, T2.Justificacion, T2.Viable, T3.Id_Proyecto, T3.NomProyecto FROM Convocatoria T1, ConvocatoriaProyecto T2, Proyecto T3 WHERE T1.Id_Convocatoria=T2.CodConvocatoria AND T2.CodConvocatoria=T3.Id_Proyecto AND T2.CodConvocatoria=50