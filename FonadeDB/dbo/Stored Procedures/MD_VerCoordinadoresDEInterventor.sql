
CREATE PROCEDURE [dbo].[MD_VerCoordinadoresDEInterventor]

AS

BEGIN

SELECT
	case when  c.inactivo = 0 then 'activo'
	else 'Inactivo' end as inactividad,
	 id_contacto,nombres + ' ' +apellidos as nombre, email, count(e.codcontacto) as Cuantos,  CAST(c.inactivo as int) inactivo
	FROM contacto c 
	LEFT JOIN Evaluador e ON id_contacto=e.codcoordinador
	INNER JOIN grupocontacto gc ON id_contacto=gc.codcontacto and codgrupo = (select Id_Grupo from Grupo where Id_Grupo=13)
	GROUP BY id_contacto,Nombres,apellidos,email,c.inactivo 
	ORDER BY nombre
	
END