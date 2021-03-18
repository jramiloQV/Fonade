﻿CREATE VIEW VW_C3_DESEMPENO_EVALUADOR (Nombres, Apellidos, Puntaje, NomItem, Id_Proyecto, NomProyecto, NomTabEvaluacion) AS SELECT T1.Nombres, T1.Apellidos, T2.Puntaje, T3.NomItem, T4.Id_Proyecto, T4.NomProyecto, T6.NomTabEvaluacion FROM Contacto T1, EvaluacionEvaluador T2, Item T3, Proyecto T4, ProyectoContacto T5, TabEvaluacion T6 WHERE T1.Id_Contacto=T5.CodContacto AND T5.CodProyecto=T4.Id_Proyecto AND T2.CodProyecto=T4.Id_Proyecto AND T2.CodItem=T3.Id_Item AND T6.CodTabEvaluacion=T3.CodTabEvaluacion AND T2.CodConvocatoria=4 AND T5.CodRol=4