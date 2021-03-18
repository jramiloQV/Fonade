﻿CREATE VIEW VW_JEFES_UNIDAD (NomCiudad, Nombres, Apellidos, Identificacion, Email, Direccion, Telefono, NomDepartamento, NomInstitucion, NomUnidad, FechaInicio, FechaFin) AS SELECT T1.NomCiudad, T2.Nombres, T2.Apellidos, T2.Identificacion, T2.Email, T2.Direccion, T2.Telefono, T3.NomDepartamento, T4.NomInstitucion, T4.NomUnidad, T5.FechaInicio, T5.FechaFin FROM Ciudad T1, Contacto T2, departamento T3, Institucion T4, InstitucionContacto T5 WHERE T1.CodDepartamento=T3.Id_Departamento AND T1.Id_Ciudad=T4.CodCiudad AND T2.Id_Contacto=T5.CodContacto AND T4.Id_Institucion=T5.CodInstitucion AND T4.Inactivo=0