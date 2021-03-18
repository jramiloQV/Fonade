
create PROCEDURE [dbo].[MD_cargarProyectosSector]

	@sector int

AS

BEGIN
	SELECT Id_Proyecto , cast(Id_Proyecto as varchar(20))+ ' - ' +  NomProyecto as proyecto from PROYECTO p 
	INNER JOIN Subsector S ON P.codsubSector=S.id_subsector and codSector=@sector
	left join Proyectocontacto pc on id_proyecto=codproyecto 
	and pc.inactivo=0 and codrol=4
	where codestado= 4
END