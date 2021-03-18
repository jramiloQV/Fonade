﻿CREATE VIEW VW_C1_PAGOS_EMPRESAS_E4 (razonsocial, Sum_T4_Id_PagoActividad, Sum_T4_CantidadDinero, Estado, Id_Proyecto, NomProyecto) AS SELECT T1.razonsocial, Sum(T4.Id_PagoActividad), Sum(T4.CantidadDinero), T4.Estado, T5.Id_Proyecto, T5.NomProyecto FROM Empresa T1, LegalizacionActa T2, LegalizacionActaProyecto T3, PagoActividad T4, Proyecto T5 WHERE T1.codproyecto=T5.Id_Proyecto AND T2.Id_Acta=T3.CodActa AND T3.Legalizado=1 AND T2.Id_Acta<=58 AND T4.CodProyecto=T5.Id_Proyecto AND T3.CodProyecto=T5.Id_Proyecto AND T4.Estado=4 GROUP BY T1.razonsocial, T4.Estado, T5.Id_Proyecto, T5.NomProyecto