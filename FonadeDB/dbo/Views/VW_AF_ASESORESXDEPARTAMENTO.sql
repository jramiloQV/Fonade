﻿CREATE VIEW VW_AF_ASESORESXDEPARTAMENTO (NomCiudad, Nombres, Apellidos, Identificacion, Genero, Email, CodInstitucion, NomDepartamento) AS SELECT T1.NomCiudad, T2.Nombres, T2.Apellidos, T2.Identificacion, T2.Genero, T2.Email, T2.CodInstitucion, T3.NomDepartamento FROM Ciudad T1, Contacto T2, departamento T3, GrupoContacto T4 WHERE T2.Id_Contacto=T4.CodContacto AND T3.Id_Departamento=T1.CodDepartamento AND T1.Id_Ciudad=T2.CodCiudad AND T4.CodGrupo=5 AND T1.CodDepartamento=11