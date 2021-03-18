﻿CREATE VIEW VW_C2_PRODUCCION1 (ValoProduccion, encargofiduciario, razonsocial, Id_Proyecto, NomProyecto) AS SELECT Sum(T1.Valor), T2.encargofiduciario, T4.razonsocial, T6.Id_Proyecto, T6.NomProyecto FROM AvanceProduccionPOMes T1, Convocatoria T2, ConvocatoriaProyecto T3, Empresa T4, InterventorProduccion T5, Proyecto T6 WHERE T1.CodProducto=T5.id_produccion AND T2.Id_Convocatoria=T3.CodConvocatoria AND T3.CodProyecto=T6.Id_Proyecto AND T4.codproyecto=T6.Id_Proyecto AND T5.CodProyecto=T6.Id_Proyecto AND T2.encargofiduciario=103937 AND T6.CodEstado>=7 GROUP BY T2.encargofiduciario, T4.razonsocial, T6.Id_Proyecto, T6.NomProyecto