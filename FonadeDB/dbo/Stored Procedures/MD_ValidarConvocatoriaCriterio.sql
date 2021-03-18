
CREATE PROCEDURE MD_ValidarConvocatoriaCriterio

	@CodConvocatoria int,
	@CONST_AsignacionRecursos int

AS

BEGIN

	select case when count(codproyecto)>0 then 0 else 1 end as Conteo from proyecto, convocatoriaproyecto
	where id_proyecto=codproyecto and codestado>=@CONST_AsignacionRecursos
	and codconvocatoria=@CodConvocatoria

END