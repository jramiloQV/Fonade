﻿CREATE VIEW VW_CONVREGIONALESCONTACTOS (NomCiudad, Nombres, Apellidos, Identificacion, Email, Direccion, Telefono, CodConvocatoria, NomDepartamento, NomInstitucion, NomUnidad, Id_Proyecto, NomProyecto, CodRol, Recursos, Fecha, NomSector, NomSubSector) AS SELECT T1.NomCiudad, T2.Nombres, T2.Apellidos, T2.Identificacion, T2.Email, T2.Direccion, T2.Telefono, T3.CodConvocatoria, T4.NomDepartamento, T5.NomInstitucion, T5.NomUnidad, T6.Id_Proyecto, T6.NomProyecto, T7.CodRol, T8.Recursos, T9.Fecha, T10.NomSector, T11.NomSubSector FROM Ciudad T1, Contacto T2, ConvocatoriaProyecto T3, departamento T4, Institucion T5, Proyecto T6, ProyectoContacto T7, ProyectoFinanzasIngresos T8, ProyectoFormalizacion T9, Sector T10, SubSector T11 WHERE T1.CodDepartamento=T4.Id_Departamento AND T1.Id_Ciudad=T6.CodCiudad AND T2.Id_Contacto=T7.CodContacto AND T3.CodProyecto=T6.Id_Proyecto AND T5.Id_Institucion=T6.CodInstitucion AND T7.CodProyecto=T6.Id_Proyecto AND T8.CodProyecto=T6.Id_Proyecto AND T9.codProyecto=T6.Id_Proyecto AND T11.Id_SubSector=T6.CodSubSector AND T11.CodSector=T10.Id_Sector AND T3.CodConvocatoria>=12 AND T9.CodConvocatoria>=12 AND T7.Inactivo=0