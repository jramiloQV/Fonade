﻿CREATE VIEW VW_EVALUADORESPROYECTO (Id_ProyectoContacto, CodProyecto, CodContacto, CodRol, FechaInicio, FechaFin, Inactivo, Beneficiario, Participacion, CodConvocatoria) AS SELECT T1.Id_ProyectoContacto, T1.CodProyecto, T1.CodContacto, T1.CodRol, T1.FechaInicio, T1.FechaFin, T1.Inactivo, T1.Beneficiario, T1.Participacion, T1.CodConvocatoria FROM ProyectoContacto T1 WHERE T1.CodRol=4