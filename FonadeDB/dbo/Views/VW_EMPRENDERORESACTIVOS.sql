﻿CREATE VIEW VW_EMPRENDERORESACTIVOS (Nombres, Apellidos, Identificacion, Email, CodInstitucion, Clave, CodProyecto, CodRol, FechaInicio, FechaFin) AS SELECT T1.Nombres, T1.Apellidos, T1.Identificacion, T1.Email, T1.CodInstitucion, T1.Clave, T2.CodProyecto, T2.CodRol, T2.FechaInicio, T2.FechaFin FROM Contacto T1, ProyectoContacto T2 WHERE T1.Id_Contacto=T2.CodContacto AND T2.CodRol=3 AND T1.Inactivo=0 AND T2.Inactivo=0