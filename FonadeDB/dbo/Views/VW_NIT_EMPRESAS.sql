﻿CREATE VIEW VW_NIT_EMPRESAS (NomCiudad, Nombres, Apellidos, NomDepartamento, razonsocial, DomicilioEmpresa, Telefono, Email, Nit, Id_Proyecto, NomProyecto) AS SELECT T1.NomCiudad, T2.Nombres, T2.Apellidos, T3.NomDepartamento, T4.razonsocial, T4.DomicilioEmpresa, T4.Telefono, T4.Email, T4.Nit, T6.Id_Proyecto, T6.NomProyecto FROM Ciudad T1, Contacto T2, departamento T3, Empresa T4, EmpresaContacto T5, Proyecto T6 WHERE T1.Id_Ciudad=T4.CodCiudad AND T1.CodDepartamento=T3.Id_Departamento AND T2.Id_Contacto=T5.codcontacto AND T5.RepresentanteLegal=1 AND T4.id_empresa=T5.codempresa AND T4.codproyecto=T6.Id_Proyecto