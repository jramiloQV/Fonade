﻿CREATE VIEW VW_TAREAPLANDENEGOCIO (Id_TareaUsuario, CodTareaPrograma, CodContacto, CodProyecto, NomTareaUsuario, Descripcion, Recurrente, RecordatorioEmail, NivelUrgencia, RecordatorioPantalla, RequiereRespuesta, CodContactoAgendo, DocumentoRelacionado, Id_TareaUsuarioRepeticion, Fecha, CodTareaUsuario, Parametros, Respuesta, FechaCierre, GUID) AS SELECT * FROM TareaUsuario T1, TareaUsuarioRepeticion T2 WHERE T1.Id_TareaUsuario=T2.CodTareaUsuario AND T1.CodProyecto=51008