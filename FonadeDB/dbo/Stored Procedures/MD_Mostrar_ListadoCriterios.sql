
CREATE PROCEDURE [dbo].[MD_Mostrar_ListadoCriterios]

	@IdConvocaoria int

AS

BEGIN

	select id_criterio, nomcriterio from CriterioPriorizacion 
	where not exists 
	(
	select codcriteriopriorizacion from convocatoriacriteriopriorizacion
	where id_criterio=codcriteriopriorizacion and codconvocatoria = @IdConvocaoria
	)
	order by nomcriterio

END