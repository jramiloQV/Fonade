﻿CREATE VIEW VW_OBSERVACIONES_EVALUACION (CodProyecto, NombreConvocatoria, Nombres, Email, Perfil, Comentarios, Fecha, NomProyecto) AS SELECT T1.CodProyecto, T1.NombreConvocatoria, T1.Nombres, T1.Email, T1.Perfil, T1.Comentarios, T1.Fecha, T2.NomProyecto FROM ObservacionesEvaluacion T1, Proyecto T2 WHERE T1.CodProyecto=T2.Id_Proyecto AND T1.Codconvocatoria=316