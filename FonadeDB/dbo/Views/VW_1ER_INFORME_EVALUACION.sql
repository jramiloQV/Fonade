﻿CREATE VIEW VW_1ER_INFORME_EVALUACION (CodConvocatoria, Justificacion, Viable, ValorRecomendado, Id_Proyecto, NomProyecto) AS SELECT T1.CodConvocatoria, T1.Justificacion, T1.Viable, T2.ValorRecomendado, T3.Id_Proyecto, T3.NomProyecto FROM ConvocatoriaProyecto T1, EvaluacionObservacion T2, Proyecto T3 WHERE T1.CodProyecto=T3.Id_Proyecto AND T2.CodProyecto=T3.Id_Proyecto AND T1.CodConvocatoria=T2.CodConvocatoria AND T1.CodConvocatoria>=90