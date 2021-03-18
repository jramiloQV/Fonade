

CREATE PROCEDURE [dbo].[MD_VeIntervSector]

AS

BEGIN

	Select Id_Contacto, Nombres + ' ' + Apellidos as nombre
	FROM Contacto c 
	JOIN Interventor e ON id_contacto=e.codcontacto 
	JOIN InterventorContrato ec ON id_contacto=ec.codcontacto 
	inner join GrupoContacto gc on ID_Contacto = gc.CodContacto 
	left join proyectocontacto pc on ID_Contacto = pc.CodContacto 
	and pc.codrol = 6 and pc.inactivo=0 
	and c.Id_Contacto = '77963'
	WHERE  c.inactivo=0 AND CodGrupo = 13
	group by id_contacto, Nombres, Apellidos
	order by Id_Contacto

END