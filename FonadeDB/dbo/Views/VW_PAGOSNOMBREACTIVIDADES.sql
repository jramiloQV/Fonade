﻿CREATE VIEW VW_PAGOSNOMBREACTIVIDADES (razonsocial, Id_PagoActividad, NomPagoActividad, CantidadDinero, FechaRtaFA, ObservacionesFA, valorretefuente, valorreteiva, valorreteica, otrosdescuentos, valorpagado, fechapago, NumIdentificacion, Nombre, Apellido, Id_Proyecto, NomProyecto) AS SELECT T1.razonsocial, T2.Id_PagoActividad, T2.NomPagoActividad, T2.CantidadDinero, T2.FechaRtaFA, T2.ObservacionesFA, T2.valorretefuente, T2.valorreteiva, T2.valorreteica, T2.otrosdescuentos, T2.valorpagado, T2.fechapago, T3.NumIdentificacion, T3.Nombre, T3.Apellido, T4.Id_Proyecto, T4.NomProyecto FROM Empresa T1, PagoActividad T2, PagoBeneficiario T3, Proyecto T4 WHERE T1.codproyecto=T4.Id_Proyecto AND T2.CodProyecto=T4.Id_Proyecto AND T2.CodPagoBeneficiario=T3.Id_PagoBeneficiario AND T2.Estado=4 AND T4.Id_Proyecto=6668