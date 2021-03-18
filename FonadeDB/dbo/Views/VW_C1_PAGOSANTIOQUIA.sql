﻿CREATE VIEW VW_C1_PAGOSANTIOQUIA (NomCiudad, encargofiduciario, razonsocial, No_Desembolsos, Valor_Desembolsado, Id_Proyecto, NomProyecto) AS SELECT T1.NomCiudad, T2.encargofiduciario, T4.razonsocial, Count(T5.Id_PagoActividad), Sum(T5.CantidadDinero), T6.Id_Proyecto, T6.NomProyecto FROM Ciudad T1, Convocatoria T2, ConvocatoriaProyecto T3, Empresa T4, PagoActividad T5, Proyecto T6 WHERE T1.Id_Ciudad=T4.CodCiudad AND T2.Id_Convocatoria=T3.CodConvocatoria AND T3.CodProyecto=T6.Id_Proyecto AND T4.codproyecto=T6.Id_Proyecto AND T4.codproyecto=T5.CodProyecto AND T1.CodDepartamento=5 AND T5.Estado=4 GROUP BY T1.NomCiudad, T2.encargofiduciario, T4.razonsocial, T6.Id_Proyecto, T6.NomProyecto