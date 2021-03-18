﻿CREATE VIEW VW_PAGOS_1_CONVOCATORIA (razonsocial, No_Desembolsos, Valor_Desembolsado, Id_Proyecto, NomProyecto) AS SELECT T3.razonsocial, Count(T4.Id_PagoActividad), Sum(T4.CantidadDinero), T5.Id_Proyecto, T5.NomProyecto FROM Convocatoria T1, ConvocatoriaProyecto T2, Empresa T3, PagoActividad T4, Proyecto T5 WHERE T1.Id_Convocatoria=T2.CodConvocatoria AND T2.CodProyecto=T5.Id_Proyecto AND T3.codproyecto=T5.Id_Proyecto AND T4.CodProyecto=T5.Id_Proyecto AND T1.encargofiduciario=103806 AND T4.Estado=4 GROUP BY T3.razonsocial, T5.Id_Proyecto, T5.NomProyecto