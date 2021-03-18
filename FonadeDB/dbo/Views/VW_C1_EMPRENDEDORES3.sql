﻿CREATE VIEW VW_C1_EMPRENDEDORES3 (CodEstado, Contacto, Rol, CodigoProyecto, NombreProyecto, CodConvocatoria, Viable) AS SELECT T1.CodEstado, T2.Contacto, T2.Rol, T2.CodigoProyecto, T2.NombreProyecto, T3.CodConvocatoria, T3.Viable FROM Proyecto T1, ContactosRol T2, ConvocatoriaProyecto T3 WHERE T2.CodigoProyecto=T3.CodProyecto AND T3.CodConvocatoria=1 AND T1.CodEstado=5 AND T1.Id_Proyecto=T2.CodigoProyecto