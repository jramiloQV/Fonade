﻿CREATE VIEW VW_UEMPRENDIMIENTOJESSI (NomCiudad, Nombres, Apellidos, Identificacion, Email, Direccion, Telefono, NomDepartamento, NomInstitucion, NomUnidad, Nit, DIRECCIONUNIDAD, TELUNIDAD, FechaInicio, FechaFin) AS SELECT T1.NomCiudad, T2.Nombres, T2.Apellidos, T2.Identificacion, T2.Email, T2.Direccion, T2.Telefono, T3.NomDepartamento, T4.NomInstitucion, T4.NomUnidad, T4.Nit, T4.Direccion, T4.Telefono, T5.FechaInicio, T5.FechaFin FROM Ciudad T1, Contacto T2, departamento T3, Institucion T4, InstitucionContacto T5 WHERE T4.Id_Institucion=T5.CodInstitucion AND T1.CodDepartamento=T3.Id_Departamento AND T2.Id_Contacto=T5.CodContacto AND T1.Id_Ciudad=T4.CodCiudad