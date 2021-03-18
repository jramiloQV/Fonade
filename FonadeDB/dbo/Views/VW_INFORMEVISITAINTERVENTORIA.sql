﻿CREATE VIEW VW_INFORMEVISITAINTERVENTORIA (razonsocial, codproyecto, Valor, NombreInforme, FechaSalida, FechaRegreso, InformacionTecnica, InformacionFinanciera, NomMedioDeTransporte) AS SELECT T1.razonsocial, T1.codproyecto, T2.Valor, T3.NombreInforme, T3.FechaSalida, T3.FechaRegreso, T3.InformacionTecnica, T3.InformacionFinanciera, T4.NomMedioDeTransporte FROM Empresa T1, InformeMedioTransporte T2, InformeVisitaInterventoria T3, MedioDeTransporte T4, Visita T5 WHERE T1.id_empresa=T5.Id_Empresa AND T2.CodInforme=T3.Id_Informe AND T3.CodEmpresa=T1.id_empresa AND T4.Id_MedioDeTransporte=T2.CodMedioTransporte