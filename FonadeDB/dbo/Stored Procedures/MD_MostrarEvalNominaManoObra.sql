
CREATE PROCEDURE [dbo].[MD_MostrarEvalNominaManoObra]

	@CodProyecto int

AS

BEGIN

	select id_insumo,GeneradoPrimerAno,sueldomes 
	from proyectoinsumo,proyectoempleomanoobra 
	where id_insumo=codmanoobra and codtipoinsumo=2 
	and codproyecto=@CodProyecto order by id_insumo

END