﻿
CREATE VIEW VW_C_EVALUADORES (Contacto, email, CodigoProyecto, NombreProyecto, CodConvocatoria, Viable) AS SELECT T1.Contacto, T1.email, T1.CodigoProyecto, T1.NombreProyecto, T2.CodConvocatoria, T2.Viable FROM ContactosRol T1, ConvocatoriaProyecto T2 WHERE T1.CodigoProyecto=T2.CodProyecto AND T2.CodConvocatoria=1