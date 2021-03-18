﻿CREATE VIEW VW_Ventas_Ejecucion_Valor (No_Productos, Cantidad, razonsocial, Id_Proyecto, NomProyecto) AS SELECT Count(T1.CodProducto), Sum(T1.Valor), T2.razonsocial, T4.Id_Proyecto, T4.NomProyecto FROM AvanceVentasPOMes T1, Empresa T2, InterventorVentas T3, Proyecto T4 WHERE T1.CodProducto=T3.id_ventas AND T2.codproyecto=T4.Id_Proyecto AND T3.CodProyecto=T4.Id_Proyecto AND T1.CodTipoFinanciacion=1 AND T4.CodEstado=7 AND T1.Mes<=4 GROUP BY T2.razonsocial, T4.Id_Proyecto, T4.NomProyecto