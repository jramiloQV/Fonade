﻿CREATE VIEW VW_C2_CONTACTOS_PROYECTO (identificacion, Contacto, email, Rol, CodigoProyecto, NombreProyecto, CodConvocatoria, Viable) AS SELECT T1.identificacion, T1.Contacto, T1.email, T1.Rol, T1.CodigoProyecto, T1.NombreProyecto, T2.CodConvocatoria, T2.Viable FROM ContactosRol T1, ConvocatoriaProyecto T2 WHERE T1.CodigoProyecto=T2.CodProyecto AND T2.CodConvocatoria=2 AND T2.Viable=0 AND T1.Rol=1