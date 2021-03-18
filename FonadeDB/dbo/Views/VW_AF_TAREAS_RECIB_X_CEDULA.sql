﻿CREATE VIEW VW_AF_TAREAS_RECIB_X_CEDULA (Nombres, Apellidos, Identificacion, CodContacto, CodProyecto, NomTareaUsuario, Descripcion, Recurrente, RecordatorioEmail, NivelUrgencia, RecordatorioPantalla, RequiereRespuesta, CodContactoAgendo, DocumentoRelacionado, Fecha, Respuesta, FechaCierre) AS SELECT T1.Nombres, T1.Apellidos, T1.Identificacion, T2.CodContacto, T2.CodProyecto, T2.NomTareaUsuario, T2.Descripcion, T2.Recurrente, T2.RecordatorioEmail, T2.NivelUrgencia, T2.RecordatorioPantalla, T2.RequiereRespuesta, T2.CodContactoAgendo, T2.DocumentoRelacionado, T3.Fecha, T3.Respuesta, T3.FechaCierre FROM Contacto T1, TareaUsuario T2, TareaUsuarioRepeticion T3 WHERE T1.Id_Contacto=T2.CodContacto AND T2.Id_TareaUsuario=T3.CodTareaUsuario AND T1.Identificacion=14676491