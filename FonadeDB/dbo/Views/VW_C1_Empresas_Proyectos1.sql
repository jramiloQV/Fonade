﻿CREATE VIEW VW_C1_Empresas_Proyectos1 (NomCiudad, Nombres, Apellidos, Email, Direccion, Telefono, CodConvocatoria, NomDepartamento, razonsocial, Estado, Id_Proyecto, NomProyecto) AS SELECT T1.NomCiudad, T2.Nombres, T2.Apellidos, T2.Email, T2.Direccion, T2.Telefono, T3.CodConvocatoria, T4.NomDepartamento, T5.razonsocial, T6.Estado, T7.Id_Proyecto, T7.NomProyecto FROM Ciudad T1, Contacto T2, ConvocatoriaProyecto T3, departamento T4, Empresa T5, PagoActividad T6, Proyecto T7, ProyectoContacto T8 WHERE T2.Id_Contacto=T8.CodContacto AND T8.CodProyecto=T7.Id_Proyecto AND T3.CodProyecto=T7.Id_Proyecto AND T1.Id_Ciudad=T7.CodCiudad AND T1.CodDepartamento=T4.Id_Departamento AND T5.codproyecto=T7.Id_Proyecto AND T5.codproyecto=T6.Id_PagoActividad AND T3.CodConvocatoria=1 AND T8.CodRol=3 AND T6.Estado>=0