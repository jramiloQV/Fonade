﻿CREATE VIEW VW_DGSG (Id_Contacto, Nombres, Apellidos, Email, Clave) AS SELECT T1.Id_Contacto, T1.Nombres, T1.Apellidos, T1.Email, T1.Clave FROM Contacto T1 WHERE T1.Id_Contacto=1079175970