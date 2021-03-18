﻿CREATE VIEW VW_CONSULTA_UNIDADESEMPRENDIMIENTO (NomCiudad, Nombres, Apellidos, Identificacion, Email, Telefono, NomDepartamento, NomInstitucion, NomUnidad, Nit, RegistroIcfes, Direccion, TelUnidadEmprendimiento, FechaInicio, FechaFin, MotivoCambioJefe) AS SELECT T1.NomCiudad, T2.Nombres, T2.Apellidos, T2.Identificacion, T2.Email, T2.Telefono, T3.NomDepartamento, T4.NomInstitucion, T4.NomUnidad, T4.Nit, T4.RegistroIcfes, T4.Direccion, T4.Telefono, T5.FechaInicio, T5.FechaFin, T5.MotivoCambioJefe FROM Ciudad T1, Contacto T2, departamento T3, Institucion T4, InstitucionContacto T5 WHERE T1.CodDepartamento=T3.Id_Departamento AND T1.Id_Ciudad=T4.CodCiudad AND T2.Id_Contacto=T5.CodContacto AND T4.Id_Institucion=T5.CodInstitucion