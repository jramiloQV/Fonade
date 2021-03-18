﻿CREATE VIEW VW_INTERVENTOR_EMPRENDEDOR4 (Nombres, Apellidos, N1, A1, razonsocial, Id_Proyecto, NomProyecto, NomTareaUsuario, Descripcion, Fecha, Respuesta, FechaCierre) AS SELECT T1.Nombres, T1.Apellidos, T2.Nombres, T2.Apellidos, T3.razonsocial, T5.Id_Proyecto, T5.NomProyecto, T7.NomTareaUsuario, T7.Descripcion, T8.Fecha, T8.Respuesta, T8.FechaCierre FROM Contacto T1, Contacto T2, Empresa T3, EmpresaInterventor T4, Proyecto T5, ProyectoContacto T6, TareaUsuario T7, TareaUsuarioRepeticion T8 WHERE T7.CodContacto=T1.Id_Contacto AND T1.Id_Contacto=T6.CodContacto AND T6.CodProyecto=T5.Id_Proyecto AND T7.CodProyecto=T5.Id_Proyecto AND T7.CodContactoAgendo=T2.Id_Contacto AND T4.CodContacto=T2.Id_Contacto AND T6.CodProyecto=T3.codproyecto AND T3.id_empresa=T4.CodEmpresa AND T7.Id_TareaUsuario=T8.CodTareaUsuario AND T3.codproyecto=T5.Id_Proyecto AND T6.CodRol=3 AND T4.Rol=8 AND T5.Id_Proyecto=333 AND T6.Inactivo=0 AND T4.Inactivo=0