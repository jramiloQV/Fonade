﻿CREATE VIEW VW_CONSULTAPLANESPABLO (Nombres, Apellidos, Identificacion, Email, ValorRecomendado, Id_Proyecto, NomProyecto, FechaCreacion, CodRol, FechaInicio, FechaFin, Inactivo) AS SELECT T1.Nombres, T1.Apellidos, T1.Identificacion, T1.Email, T2.ValorRecomendado, T3.Id_Proyecto, T3.NomProyecto, T3.FechaCreacion, T4.CodRol, T4.FechaInicio, T4.FechaFin, T4.Inactivo FROM Contacto T1, EvaluacionObservacion T2, Proyecto T3, ProyectoContacto T4 WHERE T1.Id_Contacto=T4.CodContacto AND T2.CodProyecto=T3.Id_Proyecto AND T4.CodProyecto=T3.Id_Proyecto AND T4.CodRol<=3