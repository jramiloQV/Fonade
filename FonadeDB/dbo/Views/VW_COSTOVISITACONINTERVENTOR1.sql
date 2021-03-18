﻿CREATE VIEW VW_COSTOVISITACONINTERVENTOR1 (Nombres, Apellidos, NomCiudad, razonsocial, codproyecto, Valor, NombreInforme, FechaSalida, FechaRegreso, CostoVisita, NomMedioDeTransporte) AS SELECT T1.Nombres, T1.Apellidos, T3.NomCiudad, T4.razonsocial, T4.codproyecto, T5.Valor, T6.NombreInforme, T6.FechaSalida, T6.FechaRegreso, T6.CostoVisita, T7.NomMedioDeTransporte FROM Contacto T1, EmpresaInterventor T2, Ciudad T3, Empresa T4, InformeMedioTransporte T5, InformeVisitaInterventoria T6, MedioDeTransporte T7 WHERE T1.Id_Contacto=T2.CodContacto AND T2.CodEmpresa=T4.id_empresa AND T3.Id_Ciudad=T6.CodCiudadDestino AND T5.CodMedioTransporte=T7.Id_MedioDeTransporte AND T5.CodInforme=T6.Id_Informe AND T6.CodEmpresa=T4.id_empresa