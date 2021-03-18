﻿CREATE VIEW VW_AF_FECHA_CARGA_DE_ARCHIVOS_X_PAGO (NomConvocatoria, CodProyecto, Id_PagoActividad, Fecha) AS SELECT T1.NomConvocatoria, T2.CodProyecto, T3.Id_PagoActividad, T4.Fecha FROM Convocatoria T1, ConvocatoriaProyecto T2, PagoActividad T3, PagoActividadarchivo T4 WHERE T1.Id_Convocatoria=T2.CodConvocatoria AND T2.CodProyecto=T3.CodProyecto AND T3.Id_PagoActividad=T4.CodPagoActividad AND T1.CodConvenio=1 AND T2.CodProyecto>45000