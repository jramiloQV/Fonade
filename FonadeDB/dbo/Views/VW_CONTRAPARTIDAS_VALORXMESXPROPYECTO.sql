﻿CREATE VIEW VW_CONTRAPARTIDAS_VALORXMESXPROPYECTO (NomActividad, CodProyecto, Item, Mes, CodTipoFinanciacion, Valor) AS SELECT T1.NomActividad, T1.CodProyecto, T1.Item, T2.Mes, T2.CodTipoFinanciacion, T2.Valor FROM ProyectoActividadPOInterventor T1, ProyectoActividadPOMesInterventor T2 WHERE T1.Id_Actividad=T2.CodActividad AND T1.Item=96