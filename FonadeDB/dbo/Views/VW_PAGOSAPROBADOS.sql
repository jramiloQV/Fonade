﻿CREATE VIEW VW_PAGOSAPROBADOS (Id_PagoActividad, NomPagoActividad, Tipo, Mes, CodPagoBeneficiario, Observaciones, CantidadDinero, CodActividad, CodProyecto, Estado, FechaIngreso, FechaInterventor, FechaCoordinador, FechaRtaFA, RutaArchivoZIP, ObservacionesFA, valorretefuente, valorreteiva, valorreteica, otrosdescuentos, valorpagado, fechapago, fecharechazo) AS SELECT T1.Id_PagoActividad, T1.NomPagoActividad, T1.Tipo, T1.Mes, T1.CodPagoBeneficiario, T1.Observaciones, T1.CantidadDinero, T1.CodActividad, T1.CodProyecto, T1.Estado, T1.FechaIngreso, T1.FechaInterventor, T1.FechaCoordinador, T1.FechaRtaFA, T1.RutaArchivoZIP, T1.ObservacionesFA, T1.valorretefuente, T1.valorreteiva, T1.valorreteica, T1.otrosdescuentos, T1.valorpagado, T1.fechapago, T1.fecharechazo FROM PagoActividad T1 WHERE T1.Estado=4