
CREATE PROCEDURE MD_Mostrar_ConvocatoriaCriterioPriorizacion

	@IdConvocaoria int

AS

BEGIN

	SELECT id_criterio, nomcriterio, ccp.parametros, incidencia
	FROM convocatoriacriteriopriorizacion ccp,  criteriopriorizacion 
	WHERE id_criterio=codcriteriopriorizacion and codConvocatoria=@IdConvocaoria

END