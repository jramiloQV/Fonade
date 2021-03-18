Create PROCEDURE [dbo].[MD_cargarProyectoContacto]

	@codContacto int

AS

BEGIN
	select NomProyecto, Sumario from proyecto where CodContacto=@codContacto
END