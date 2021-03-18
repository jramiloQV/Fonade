﻿CREATE VIEW VW_INTERVENTORIA_2 (id_convocatoria, NomConvocatoria, AñoConvocatoria, id_proyecto, NomProyecto, NomSector, NomSubsector, ValorRecomendado, NomEstado, NomCiudad, NomDepartamento, DomicilioEmpresa, TelefonoEmpresa, EmailEmpresa, NomUnidad, Identificacion, Nombres, Apellidos, Direccion, Telefono, Email) AS SELECT T1.id_convocatoria, T1.NomConvocatoria, T1.AñoConvocatoria, T1.id_proyecto, T1.NomProyecto, T1.NomSector, T1.NomSubsector, T1.ValorRecomendado, T1.NomEstado, T1.NomCiudad, T1.NomDepartamento, T1.DomicilioEmpresa, T1.TelefonoEmpresa, T1.EmailEmpresa, T1.NomUnidad, T1.Identificacion, T1.Nombres, T1.Apellidos, T1.Direccion, T1.Telefono, T1.Email FROM ConvocatoriaProyectoDtosGrales T1 WHERE T1.id_convocatoria>=101 AND T1.Viable>=1