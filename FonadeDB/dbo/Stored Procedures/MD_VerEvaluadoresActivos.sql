
CREATE PROCEDURE [dbo].[MD_VerEvaluadoresActivos]

AS

BEGIN
	SELECT distinct Id_Contacto, Nombres + ' ' +Apellidos as nombre from Contacto c 
	inner join EvaluadorContrato ec on id_contacto = ec.codcontacto  
	inner join Evaluador e on id_contacto = e.codcontacto  
	inner join ProyectoContacto PC on id_contacto = pc.codcontacto  
	inner join grupocontacto gc on id_contacto = gc.codcontacto and codgrupo= (select Id_Grupo from Grupo where NomGrupo='Evaluador')
	WHERE c.inactivo = 0 
	order by nombre
END