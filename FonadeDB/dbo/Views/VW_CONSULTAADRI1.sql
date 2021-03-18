﻿CREATE VIEW VW_CONSULTAADRI1 (NomCiudad, Nombres, Apellidos, Identificacion, Email, Direccion, Telefono, NomDepartamento, Id_Proyecto, NomProyecto) AS SELECT T1.NomCiudad, T2.Nombres, T2.Apellidos, T2.Identificacion, T2.Email, T2.Direccion, T2.Telefono, T3.NomDepartamento, T4.Id_Proyecto, T4.NomProyecto FROM Ciudad T1, Contacto T2, departamento T3, Proyecto T4, ProyectoContacto T5 WHERE T1.Id_Ciudad=T4.CodCiudad AND T1.CodDepartamento=T3.Id_Departamento AND T2.Id_Contacto=T5.CodContacto AND T4.Id_Proyecto=T5.CodProyecto AND T5.Inactivo=0 AND T5.CodRol=3