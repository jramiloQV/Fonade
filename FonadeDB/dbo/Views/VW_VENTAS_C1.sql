﻿CREATE VIEW VW_VENTAS_C1 (Valor_Ventas, encargofiduciario, razonsocial, Id_Proyecto, NomProyecto) AS SELECT Sum(T1.Valor), T2.encargofiduciario, T4.razonsocial, T6.Id_Proyecto, T6.NomProyecto FROM AvanceVentasPOMes T1, Convocatoria T2, ConvocatoriaProyecto T3, Empresa T4, InterventorVentas T5, Proyecto T6 WHERE T1.CodProducto=T5.id_ventas AND T2.Id_Convocatoria=T3.CodConvocatoria AND T3.CodProyecto=T4.codproyecto AND T4.codproyecto=T6.Id_Proyecto AND T5.CodProyecto=T4.codproyecto AND T2.encargofiduciario=103806 AND T1.CodTipoFinanciacion=2 GROUP BY T2.encargofiduciario, T4.razonsocial, T6.Id_Proyecto, T6.NomProyecto