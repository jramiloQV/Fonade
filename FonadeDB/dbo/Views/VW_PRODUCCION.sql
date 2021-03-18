﻿CREATE VIEW VW_PRODUCCION (razonsocial, Sum_T3_Valor, Id_Proyecto, NomProyecto) AS SELECT T1.razonsocial, Sum(T3.Valor), T4.Id_Proyecto, T4.NomProyecto FROM Empresa T1, InterventorProduccion T2, InterventorProduccionMes T3, Proyecto T4, ProyectoProducto T5 WHERE T1.codproyecto=T2.CodProyecto AND T3.CodProducto=T5.Id_Producto AND T1.codproyecto=T4.Id_Proyecto AND T4.Id_Proyecto=T5.CodProyecto AND T3.Tipo=1 AND T3.Mes<=6 GROUP BY T1.razonsocial, T4.Id_Proyecto, T4.NomProyecto