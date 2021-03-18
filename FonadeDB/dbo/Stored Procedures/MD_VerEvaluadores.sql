
CREATE PROCEDURE [dbo].[MD_VerEvaluadores]

AS

BEGIN

	SELECT
	case when  c.inactivo = 0 then 'activo'
		else 'Inactivo' end as inactividad,
		 id_contacto,nombres + ' ' +apellidos as nombre, email, count(distinct id_proyecto) as Cuantos, CAST(c.inactivo as int) inactivo, 
	count(distinct ec.numcontrato) as Inhabilitado 
	FROM contacto c 
	LEFT JOIN Evaluador e ON id_contacto=e.codcontacto 
	left join evaluadorcontrato ec on id_contacto=ec.codcontacto 
	INNER JOIN grupocontacto gc ON id_contacto=gc.codcontacto and codgrupo = (select Id_Grupo from Grupo where NomGrupo='Evaluador')
	LEFT JOIN proyectocontacto pc  ON id_contacto=pc.codcontacto and pc.inactivo=0 and codrol = (select Id_Rol from Rol where Nombre='Evaluador')
	LEFT JOIN proyecto p ON id_proyecto=pc.codproyecto and codestado= 4
	GROUP BY id_contacto,Nombres,apellidos,email,c.inactivo 
	ORDER BY nombre
	
END