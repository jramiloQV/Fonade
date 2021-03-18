﻿CREATE VIEW VW_01_Vtas_Proyectadas (TotalVentas, CodConvocatoria, Id_Proyecto) AS SELECT T1.TotalVentas, T3.CodConvocatoria, T4.Id_Proyecto FROM ProyectoProyeccionesVentasTotal T1, Convocatoria T2, ConvocatoriaProyecto T3, Proyecto T4 WHERE T1.codproyecto=T4.Id_Proyecto AND T2.Id_Convocatoria=T3.CodConvocatoria AND T3.CodProyecto=T4.Id_Proyecto AND T3.CodConvocatoria>=90