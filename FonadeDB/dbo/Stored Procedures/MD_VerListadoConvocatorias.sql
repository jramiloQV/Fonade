
CREATE PROCEDURE MD_VerListadoConvocatorias
	
AS

BEGIN

	SELECT id_convocatoria, nomConvocatoria,CONVERT(VARCHAR(11), FechaInicio, 100) AS FInicio, FechaInicio ,CONVERT(VARCHAR, FechaFin, 100) as FFin, FechaFin, CAST(Publicado as int) as Publicado
	FROM Convocatoria 

END