﻿CREATE VIEW VW_C2_INTERVENTORES (Nombres, Apellidos, Email, razonsocial, Rol, Id_Proyecto, NomProyecto) AS SELECT T1.Nombres, T1.Apellidos, T1.Email, T3.razonsocial, T4.Rol, T5.Id_Proyecto, T5.NomProyecto FROM Contacto T1, ConvocatoriaProyecto T2, Empresa T3, EmpresaInterventor T4, Proyecto T5 WHERE T1.Id_Contacto=T4.CodContacto AND T2.CodProyecto=T3.codproyecto AND T4.CodEmpresa=T3.id_empresa AND T5.Id_Proyecto=T3.codproyecto AND T2.CodConvocatoria=2 AND T4.Inactivo=0