
create PROCEDURE [dbo].[MD_cargarProyectoSumarioActual]

	@codproyecto int

AS

BEGIN
	select NomProyecto, Sumario from proyecto where Id_Proyecto=@codproyecto
END