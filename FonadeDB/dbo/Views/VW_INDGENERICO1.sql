﻿CREATE VIEW VW_INDGENERICO1 (razonsocial, Aspecto, FechaSeguimiento, Numerador, Denominador, Descripcion, RangoAceptable, CodTipoIndicadorInter, Observacion, Id_Proyecto, NomProyecto, NomTipoIndicadorInter) AS SELECT T1.razonsocial, T2.Aspecto, T2.FechaSeguimiento, T2.Numerador, T2.Denominador, T2.Descripcion, T2.RangoAceptable, T2.CodTipoIndicadorInter, T2.Observacion, T3.Id_Proyecto, T3.NomProyecto, T4.NomTipoIndicadorInter FROM Empresa T1, InterventorIndicador T2, Proyecto T3, TipoIndicadorInter T4 WHERE T1.codproyecto=T2.CodProyecto AND T2.CodProyecto=T3.Id_Proyecto AND T2.CodTipoIndicadorInter=T4.Id_TipoIndicadorInter AND T4.Id_TipoIndicadorInter=2