﻿CREATE VIEW VW_C3_VENTAS_PROYECTADAS (TotalVentas, encargofiduciario, razonsocial, Id_Proyecto, NomProyecto) AS SELECT T1.TotalVentas, T2.encargofiduciario, T4.razonsocial, T5.Id_Proyecto, T5.NomProyecto FROM ProyectoProyeccionesVentasTotal T1, Convocatoria T2, ConvocatoriaProyecto T3, Empresa T4, Proyecto T5 WHERE T1.codproyecto=T5.Id_Proyecto AND T2.Id_Convocatoria=T3.CodConvocatoria AND T3.CodProyecto=T5.Id_Proyecto AND T4.codproyecto=T5.Id_Proyecto AND T2.encargofiduciario=103937