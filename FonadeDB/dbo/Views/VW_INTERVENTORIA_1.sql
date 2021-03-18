﻿CREATE VIEW VW_INTERVENTORIA_1 (Id_Convocatoria, NomConvocatoria, CodProyecto, Viable, NomProyecto, Sumario) AS SELECT T1.Id_Convocatoria, T1.NomConvocatoria, T2.CodProyecto, T2.Viable, T3.NomProyecto, T3.Sumario FROM Convocatoria T1, ConvocatoriaProyecto T2, Proyecto T3 WHERE T1.Id_Convocatoria=T2.CodConvocatoria AND T2.CodProyecto=T3.Id_Proyecto AND T1.Id_Convocatoria>=101 AND T2.Viable>=1