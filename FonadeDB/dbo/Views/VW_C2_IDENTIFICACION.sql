﻿CREATE VIEW VW_C2_IDENTIFICACION (identificacion, Contacto, email, Rol, CodigoProyecto, NombreProyecto, CodConvocatoria) AS SELECT T1.identificacion, T1.Contacto, T1.email, T1.Rol, T1.CodigoProyecto, T1.NombreProyecto, T2.CodConvocatoria FROM ContactosRol T1, ConvocatoriaProyecto T2 WHERE T1.CodigoProyecto=T2.CodProyecto AND T2.CodConvocatoria=2