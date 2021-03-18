﻿CREATE VIEW VW_EVALUADORES_Y_PLANES_EVALUADOS (Nombres, Apellidos, Identificacion, Email, NomConvocatoria, Id_Proyecto, NomProyecto) AS SELECT T1.Nombres, T1.Apellidos, T1.Identificacion, T1.Email, T2.NomConvocatoria, T4.Id_Proyecto, T4.NomProyecto FROM Contacto T1, Convocatoria T2, ConvocatoriaProyecto T3, Proyecto T4, ProyectoContacto T5 WHERE T1.Id_Contacto=T5.CodContacto AND T2.Id_Convocatoria=T3.CodConvocatoria AND T3.CodProyecto=T4.Id_Proyecto AND T4.Id_Proyecto=T5.CodProyecto AND T5.CodRol=4