﻿CREATE VIEW VW_ICONOPAGOSPOREMPRESA (Id_PagoActividad) AS SELECT T2.Id_PagoActividad FROM Empresa T1, PagoActividad T2, PagoBeneficiario T3 WHERE T1.id_empresa=T3.CodEmpresa AND T2.CodPagoBeneficiario=T3.Id_PagoBeneficiario