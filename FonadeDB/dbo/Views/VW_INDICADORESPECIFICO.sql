﻿CREATE VIEW VW_INDICADORESPECIFICO (Aspecto, Numerador, Denominador, Descripcion, RangoAceptable, Observacion, Id_Proyecto, NomProyecto) AS SELECT T1.Aspecto, T1.Numerador, T1.Denominador, T1.Descripcion, T1.RangoAceptable, T1.Observacion, T2.Id_Proyecto, T2.NomProyecto FROM InterventorIndicador T1, Proyecto T2 WHERE T1.CodProyecto=T2.Id_Proyecto