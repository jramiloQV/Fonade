
CREATE PROCEDURE [dbo].[MD_VerInterventoresActivos]
 @CONST_Interventor INT
AS

BEGIN
	--SELECT distinct Id_Contacto, Nombres + ' ' +Apellidos as nombre from Contacto c 
	--inner join InterventorContrato ec on id_contacto = ec.codcontacto  
	--inner join Interventor e on id_contacto = e.codcontacto  
	--inner join ProyectoContacto PC on id_contacto = pc.codcontacto  
	--inner join grupocontacto gc on id_contacto = gc.codcontacto and codgrupo= (select Id_Grupo from Grupo where NomGrupo='Interventor')
	--WHERE c.inactivo = 0 
	--order by nombre

SELECT DISTINCT c.Id_Contacto, c.Nombres + ' ' + c.Apellidos AS nombre, e.CodCoordinador 
FROM Contacto c
INNER JOIN InterventorContrato ec ON c.Id_Contacto = ec.CodContacto 
INNER JOIN Interventor e ON c.Id_Contacto = e.CodContacto 
INNER JOIN GrupoContacto gc ON c.Id_Contacto = gc.CodContacto
  AND gc.CodGrupo =  @CONST_Interventor
WHERE (c.Inactivo = 0) 
 AND (ec.FechaExpiracion > GETDATE())
END