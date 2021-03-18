/* =============================================
Author:		Wilson Guasca
alter date: 2010-06-23 12:50
Description: Consulta única de totalización de usuarios
  ACTIVOS agrupados por perfil (grupo)

EXECUTE [ConsultaContactosActivosPorGrupo]
============================================= */
CREATE PROCEDURE [dbo].[ConsultaContactosActivosPorGrupo]
	--@FechaReferencia DateTime, 
AS
BEGIN

	SET NOCOUNT ON;
	SELECT COUNT(1) AS Cuantos, G.NomGrupo
	FROM   Contacto AS C INNER JOIN
		   GrupoContacto AS GC ON C.Id_Contacto = GC.CodContacto INNER JOIN
		   Grupo AS G ON GC.CodGrupo = G.Id_Grupo
	WHERE  (C.Inactivo = 0)
	GROUP BY G.NomGrupo
END