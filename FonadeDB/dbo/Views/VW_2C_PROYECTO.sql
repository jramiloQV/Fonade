﻿CREATE VIEW VW_2C_PROYECTO (NomCiudad, Nombres, Apellidos, NomDepartamento, ValorRecomendado, NomUnidad, Id_Proyecto, NomProyecto, CostoTotal, Nombre, Valor, TipoAporte) AS SELECT T1.NomCiudad, T2.Nombres, T2.Apellidos, T4.NomDepartamento, T5.ValorRecomendado, T6.NomUnidad, T7.Id_Proyecto, T7.NomProyecto, T7.CostoTotal, T8.Nombre, T8.Valor, T8.TipoAporte FROM Ciudad T1, Contacto T2, ConvocatoriaProyecto T3, departamento T4, EvaluacionObservacion T5, Institucion T6, Proyecto T7, ProyectoAporte T8, ProyectoContacto T9 WHERE T1.Id_Ciudad=T7.CodCiudad AND T1.CodDepartamento=T4.Id_Departamento AND T7.CodInstitucion=T6.Id_Institucion AND T3.CodProyecto=T7.Id_Proyecto AND T5.CodProyecto=T7.Id_Proyecto AND T8.CodProyecto=T7.Id_Proyecto AND T2.Id_Contacto=T9.CodContacto AND T3.CodConvocatoria=2 AND T3.Viable=1 AND T9.CodRol=3 AND T5.CodConvocatoria=2 AND T9.CodProyecto=T7.Id_Proyecto