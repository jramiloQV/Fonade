﻿
CREATE VIEW VW_C1_EVALUADORES (Nombres, Apellidos, CodConvocatoria, Viable, Id_Proyecto, NomProyecto, CodRol, Nombre) AS SELECT T1.Nombres, T1.Apellidos, T2.CodConvocatoria, T2.Viable, T3.Id_Proyecto, T3.NomProyecto, T4.CodRol, T5.Nombre FROM Contacto T1, ConvocatoriaProyecto T2, Proyecto T3, ProyectoContacto T4, Rol T5 WHERE T1.Id_Contacto=T3.CodContacto AND T1.Id_Contacto=T4.CodContacto AND T2.CodProyecto=T4.CodProyecto AND T4.CodProyecto=T3.Id_Proyecto AND T4.CodRol=4 AND T4.CodConvocatoria=1 AND T4.CodRol=T5.Id_Rol