
CREATE PROCEDURE [dbo].[MD_MostrarEvalNominaPersonal]

	@CodProyecto int

AS

BEGIN

	select id_cargo,GeneradoPrimerAno,valormensual,prestaciones =
	case 
	when GeneradoPrimerAno = 0 then  0 
	else otrosgastos/(1+(12-GeneradoPrimerAno))
	end 
	from proyectogastospersonal,proyectoempleocargo
	where id_cargo=codcargo and codproyecto=@CodProyecto order by id_cargo

END