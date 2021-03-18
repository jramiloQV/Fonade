﻿CREATE VIEW VW_SECTOR_SUBSECTOR_PROYECTO (Id_Proyecto, NomProyecto, Sumario, NomSector, NomSubSector) AS SELECT T1.Id_Proyecto, T1.NomProyecto, T1.Sumario, T2.NomSector, T3.NomSubSector FROM Proyecto T1, Sector T2, SubSector T3 WHERE T2.Id_Sector=T3.CodSector AND T3.CodSector=T2.Id_Sector