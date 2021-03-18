﻿CREATE VIEW VW_ROL (NomCiudad, Nombres, Apellidos, Email, Direccion, Telefono, Id_Convocatoria, NomConvocatoria, NomDepartamento, NomInstitucion, NomUnidad, Id_Proyecto, NomProyecto, Inactivo, Id_Rol, Nombre) AS SELECT T1.NomCiudad, T2.Nombres, T2.Apellidos, T2.Email, T2.Direccion, T2.Telefono, T3.Id_Convocatoria, T3.NomConvocatoria, T4.NomDepartamento, T5.NomInstitucion, T5.NomUnidad, T6.Id_Proyecto, T6.NomProyecto, T7.Inactivo, T8.Id_Rol, T8.Nombre FROM Ciudad T1, Contacto T2, Convocatoria T3, departamento T4, Institucion T5, Proyecto T6, ProyectoContacto T7, Rol T8 WHERE T1.CodDepartamento=T4.Id_Departamento AND T2.Id_Contacto=T7.CodContacto AND T6.CodCiudad=T1.Id_Ciudad AND T3.Id_Convocatoria=T7.CodConvocatoria AND T5.Id_Institucion=T6.CodInstitucion AND T7.CodRol=T8.Id_Rol AND T6.CodContacto=T7.CodProyecto