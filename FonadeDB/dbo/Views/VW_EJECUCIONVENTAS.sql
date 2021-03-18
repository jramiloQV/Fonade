﻿CREATE VIEW VW_EJECUCIONVENTAS (Numero_Productos, Cantidad_Vendida, Id_Proyecto, NomProyecto) AS SELECT Count(T1.CodProducto), Sum(T1.Valor), T3.Id_Proyecto, T3.NomProyecto FROM AvanceVentasPOMes T1, InterventorVentas T2, Proyecto T3 WHERE T1.CodProducto=T2.id_ventas AND T2.CodProyecto=T3.Id_Proyecto AND T1.CodTipoFinanciacion=1 AND T3.CodEstado=7 AND T1.Mes<=6 GROUP BY T3.Id_Proyecto, T3.NomProyecto