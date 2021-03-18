
CREATE PROCEDURE [dbo].[MD_MostrarProyectosPorSector]

	@CodSector int,
	@ConstRoleval int,
	@ConstEval int
AS

BEGIN

	SELECT Id_Proyecto , cast(Id_Proyecto as varchar(20))+ ' - ' +  NomProyecto as proyecto, pc.CodContacto 
	FROM Proyecto P 
	INNER JOIN Subsector S ON P.codsubSector=S.id_subsector and codSector=@CodSector
	LEFT JOIN Proyectocontacto pc on id_proyecto=codproyecto 
	and pc.inactivo=0 and codrol=@ConstRoleval
	 WHERE codestado= @ConstEval

END