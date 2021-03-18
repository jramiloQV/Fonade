﻿CREATE VIEW VW_FC_INFORMACIONINSTITUCION_SIGOB (Id_Institucion, NomInstitucion, Id_Proyecto, NomSector, NomSubSector) AS SELECT T1.Id_Institucion, T1.NomInstitucion, T2.Id_Proyecto, T3.NomSector, T4.NomSubSector FROM Institucion T1, Proyecto T2, Sector T3, SubSector T4 WHERE T1.Id_Institucion=T2.CodInstitucion AND T3.Id_Sector=T4.CodSector AND T4.Id_SubSector=T2.CodSubSector AND T2.Id_Proyecto>=49999