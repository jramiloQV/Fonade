﻿CREATE VIEW VW_MENSALES2 (Nombres, Apellidos, Identificacion, Email, Nombre1, Apellido1, Id_Proyecto, NomProyecto, NomTareaUsuario, Descripcion, Recurrente, RecordatorioEmail, NivelUrgencia, RequiereRespuesta, CodContactoAgendo, DocumentoRelacionado) AS SELECT T1.Nombres, T1.Apellidos, T1.Identificacion, T1.Email, T3.Nombres, T3.Apellidos, T4.Id_Proyecto, T4.NomProyecto, T6.NomTareaUsuario, T6.Descripcion, T6.Recurrente, T6.RecordatorioEmail, T6.NivelUrgencia, T6.RequiereRespuesta, T6.CodContactoAgendo, T6.DocumentoRelacionado FROM Contacto T1, ProyectoContacto T2, Contacto T3, Proyecto T4, ProyectoContacto T5, TareaUsuario T6 WHERE T6.CodContacto=T1.Id_Contacto AND T2.CodContacto=T1.Id_Contacto AND T2.CodRol=3 AND T4.Id_Proyecto=T2.CodProyecto AND T4.CodEstado=4 AND T6.CodContactoAgendo=T3.Id_Contacto AND T5.Id_ProyectoContacto=T3.Id_Contacto AND T5.CodRol=4 AND T5.CodProyecto=T4.Id_Proyecto