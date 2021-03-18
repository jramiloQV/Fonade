﻿CREATE VIEW VW_GTEEVALUADOR_EVALUADOR4 (Nombres, Apellidos, Nom, Ape, Id_Proyecto, NomProyecto, NomTareaUsuario, Respuesta, FechaCierre) AS SELECT T3.Nombres, T3.Apellidos, T5.Nombres, T5.Apellidos, T6.Id_Proyecto, T6.NomProyecto, T8.NomTareaUsuario, T9.Respuesta, T9.FechaCierre FROM GrupoContacto T1, GrupoContacto T2, Contacto T3, ProyectoContacto T4, Contacto T5, Proyecto T6, ProyectoContacto T7, TareaUsuario T8, TareaUsuarioRepeticion T9 WHERE T8.CodContacto=T3.Id_Contacto AND T3.Id_Contacto=T4.CodContacto AND T4.CodProyecto=T6.Id_Proyecto AND T8.CodProyecto=T6.Id_Proyecto AND T8.CodContactoAgendo=T4.CodContacto AND T7.CodContacto=T5.Id_Contacto AND T8.Id_TareaUsuario=T9.CodTareaUsuario AND T1.CodContacto=T8.CodContactoAgendo AND T2.CodContacto=T9.CodTareaUsuario AND T1.CodGrupo=9 AND T2.CodGrupo=11 AND T6.CodEstado=4