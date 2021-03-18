﻿CREATE VIEW VW_UNIDADES_EMPRENDIMIENTOXDEPTO (Id_Ciudad, NomCiudad, Nombres, Apellidos, Email, Id_Departamento, NomDepartamento, NomInstitucion, NomUnidad, No_Proyectos) AS SELECT T1.Id_Ciudad, T1.NomCiudad, T2.Nombres, T2.Apellidos, T2.Email, T3.Id_Departamento, T3.NomDepartamento, T4.NomInstitucion, T4.NomUnidad, Count(T6.Id_Proyecto) FROM Ciudad T1, Contacto T2, departamento T3, Institucion T4, InstitucionContacto T5, Proyecto T6 WHERE T1.CodDepartamento=T3.Id_Departamento AND T2.Id_Contacto=T5.CodContacto AND T1.Id_Ciudad=T4.CodCiudad AND T5.CodInstitucion=T4.Id_Institucion AND T6.CodInstitucion=T4.Id_Institucion AND T4.Inactivo=0 GROUP BY T1.Id_Ciudad, T1.NomCiudad, T2.Nombres, T2.Apellidos, T2.Email, T3.Id_Departamento, T3.NomDepartamento, T4.NomInstitucion, T4.NomUnidad