﻿CREATE VIEW VW_DIRECTORIO_EVALUADORES (Nombres, Apellidos, Identificacion, Cargo, Email, Inactivo) AS SELECT T1.Nombres, T1.Apellidos, T1.Identificacion, T1.Cargo, T1.Email, T1.Inactivo FROM Contacto T1, Evaluador T2 WHERE T1.Id_Contacto=T2.CodContacto