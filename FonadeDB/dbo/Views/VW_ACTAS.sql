﻿CREATE VIEW VW_ACTAS (Convocatoria, NumActa, NomActa, FechaActa) AS SELECT T1.NomConvocatoria, T2.NumActa, T2.NomActa, T2.FechaActa FROM Convocatoria T1, ConvocatoriaActa T2 WHERE T1.Id_Convocatoria=T2.CodConvocatoria