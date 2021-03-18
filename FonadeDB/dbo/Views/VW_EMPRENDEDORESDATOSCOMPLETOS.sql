﻿CREATE VIEW [dbo].[VW_EMPRENDEDORESDATOSCOMPLETOS] (Id_Contacto, Nombres, Apellidos, Identificacion, Email, Clave, Inactivo, NomInstitucion, NomUnidad, Id_Proyecto, NomProyecto, CodEstado, CodRol, FechaInicio, FechaFin, Inactivo2) AS SELECT T1.Id_Contacto, T1.Nombres, T1.Apellidos, T1.Identificacion, T1.Email, T1.Clave, T1.Inactivo, T2.NomInstitucion, T2.NomUnidad, T3.Id_Proyecto, T3.NomProyecto, T3.CodEstado, T4.CodRol, T4.FechaInicio, T4.FechaFin, T4.Inactivo FROM Contacto T1, Institucion T2, Proyecto T3, ProyectoContacto T4 WHERE T1.Id_Contacto=T4.CodContacto AND T3.Id_Proyecto=T4.CodProyecto AND T2.Id_Institucion=T1.CodInstitucion AND T1.Identificacion=45754111