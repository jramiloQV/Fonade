CREATE PROCEDURE [dbo].[MD_VerGerenteInterv]
--@codContacto int
AS

BEGIN



--SELECT
--	case when  c.inactivo = 0 then 'activo'
--		else 'Inactivo' end as inactividad,
--		 id_contacto,nombres + ' ' +apellidos as nombre, email, count(e.codcontacto) as Cuantos, CAST(c.inactivo as int) inactivo, 
--	count(distinct ec.numcontrato) as Inhabilitado 
--	FROM contacto c
--	LEFT JOIN interventor e ON id_contacto=e.codcoordinador 
-- left join interventorcontrato ec on id_contacto=ec.codcontacto AND ec.Motivo IS NOT NULL AND ec.FechaInicio > GETDATE() AND ec.FechaExpiracion < GETDATE()
-- INNER JOIN grupocontacto gc ON id_contacto=gc.codcontacto and codgrupo = 13
-- GROUP BY id_contacto,Nombres,apellidos,email,c.inactivo 
--ORDER BY nombre 

--ORDER BY nombre
SELECT
	case when  c.inactivo = 0 then 'activo'
		else 'Inactivo' end as inactividad,
		 id_contacto,nombres + ' ' +apellidos as nombre, email, count(e.codcontacto) as Cuantos, CAST(c.inactivo as int) inactivo, 
	count(distinct ec.numcontrato) as Inhabilitado 
	FROM contacto c
	LEFT JOIN interventor e ON id_contacto=e.codcoordinador 
 left join interventorcontrato ec on id_contacto=ec.codcontacto AND ec.Motivo IS NOT NULL AND ec.FechaInicio > GETDATE() AND ec.FechaExpiracion < GETDATE()
 INNER JOIN grupocontacto gc ON id_contacto=gc.codcontacto and codgrupo = 13
 left join proyectocontacto pc on ID_Contacto = pc.CodContacto 
	and pc.codrol = 6 and pc.inactivo=0 
 GROUP BY id_contacto,Nombres,apellidos,email,c.inactivo 
ORDER BY nombre 

END