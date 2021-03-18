﻿CREATE VIEW VW_DNP1 (NomCiudad, Id_Contacto, Nombres, Apellidos, Email, Direccion, Telefono, NomDepartamento, Id_Proyecto, NomProyecto, Sumario, Inactivo, Recursos, Id_Sector, NomSector) AS SELECT T1.NomCiudad, T2.Id_Contacto, T2.Nombres, T2.Apellidos, T2.Email, T2.Direccion, T2.Telefono, T3.NomDepartamento, T4.Id_Proyecto, T4.NomProyecto, T4.Sumario, T5.Inactivo, T6.Recursos, T7.Id_Sector, T7.NomSector FROM Ciudad T1, Contacto T2, departamento T3, Proyecto T4, ProyectoContacto T5, ProyectoFinanzasIngresos T6, Sector T7, SubSector T8 WHERE T1.CodDepartamento=T3.Id_Departamento AND T1.Id_Ciudad=T4.CodCiudad AND T2.Id_Contacto=T5.CodContacto AND T4.Id_Proyecto=T5.CodProyecto AND T4.Id_Proyecto=T6.CodProyecto AND T4.CodSubSector=T8.Id_SubSector AND T7.Id_Sector=T8.CodSector AND T4.CodEstado<=2 AND T5.Inactivo<=0 AND T5.CodRol=3