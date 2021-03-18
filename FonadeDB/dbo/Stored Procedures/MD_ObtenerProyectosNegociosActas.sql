-- =============================================
-- Author:		Alberto Palecia Benedetti
-- Create date: 14 - 03 - 2014
-- Description:	Obtiene los proyectos asociados a la convocatoria para asociarlos al acta
-- =============================================
CREATE PROCEDURE [dbo].[MD_ObtenerProyectosNegociosActas]
	@codconvocatoria int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--  se resta con 2 para no coger el tab informe
declare @count  int = (SELECT isnull(COUNT(0),0) - 2 FROM TabEvaluacion WHERE codTabEvaluacion is NULL and IdVersionProyecto = 1);

select 
	id_proyecto
	,nomproyecto 
from tabevaluacionproyecto tep,tabevaluacion te, proyecto
where id_tabevaluacion=tep.codtabevaluacion and id_proyecto=tep.codproyecto and realizado=1
and te.codtabevaluacion is null  and codestado = 4 and codconvocatoria = @codconvocatoria
and not exists (select codproyecto from evaluacionactaproyecto ep, evaluacionacta 
where id_acta=codacta and codconvocatoria = @codconvocatoria and id_proyecto=ep.codproyecto)
group by id_proyecto,nomproyecto 
HAVING count(tep.codtabevaluacion)= @count

END