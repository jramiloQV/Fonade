﻿CREATE VIEW VW_VISTACONVIVPATRICIA (CodProyecto, FechaUltimoAval, Id_Contacto, CodConvocatoria, Nombres, Apellidos, Identificacion, Email, Direccion, Telefono, NomConvocatoria) AS SELECT T1.CodProyecto, T1.FechaUltimoAval, T1.Id_Contacto, T1.CodConvocatoria, T2.Nombres, T2.Apellidos, T2.Identificacion, T2.Email, T2.Direccion, T2.Telefono, T3.NomConvocatoria FROM ContactoFechaFormalizacionProyecto T1, Contacto T2, Convocatoria T3, Institucion T4, Proyecto T5, Sector T6, SubSector T7, ContactoFechaFormalizacionProyecto T8, Contacto T9, Institucion T10, Proyecto T11, Sector T12, SubSector T13 WHERE T1.CodConvocatoria=50