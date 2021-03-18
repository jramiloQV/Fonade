﻿CREATE VIEW VW_CONSULTAINACTIVACION (Nombres, Apellidos, Identificacion, Email, Id_Proyecto, NomProyecto, FechaModificacion) AS SELECT T1.Nombres, T1.Apellidos, T1.Identificacion, T1.Email, T2.Id_Proyecto, T2.NomProyecto, T4.FechaModificacion FROM Contacto T1, Proyecto T2, ProyectoContacto T3, TabProyecto T4 WHERE T1.Id_Contacto=T3.CodContacto AND T2.Id_Proyecto=T3.CodProyecto AND T2.Id_Proyecto=T4.CodProyecto AND T2.CodEstado=1 AND T3.CodRol=3 AND T3.Inactivo=0 AND T1.Inactivo=0