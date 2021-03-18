
create PROCEDURE [dbo].[MD_VerEvalSector]

	@Sector int
AS

BEGIN

	Select Id_Contacto, Nombres + ' ' + Apellidos as nombre
	FROM Contacto c 
	JOIN Evaluador e ON id_contacto=e.codcontacto 
	JOIN EvaluadorContrato ec ON id_contacto=ec.codcontacto 
	inner join GrupoContacto gc on ID_Contacto = gc.CodContacto 
	left join proyectocontacto pc on ID_Contacto = pc.CodContacto 
	and pc.codrol = 4 and pc.inactivo=0 
	left join proyectocontacto pc2 on ID_Contacto = pc2.CodContacto 
	and pc2.codrol = 4 and pc2.inactivo=0 
	WHERE  c.inactivo=0 AND CodGrupo = 11 and exists 
	(select codsector from evaluadorsector e where id_contacto=e.codcontacto and codsector =@Sector) 
	group by id_contacto, Nombres, Apellidos
	order by Id_Contacto

END