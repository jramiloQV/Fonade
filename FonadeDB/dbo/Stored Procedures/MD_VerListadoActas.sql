
CREATE PROCEDURE [dbo].[MD_VerListadoActas]
	
AS

BEGIN

	select id_acta, numacta, nomacta, nomconvocatoria, e.publicado
	from asignacionActa e, convocatoria
	where id_convocatoria=codconvocatoria
	order by numacta

END