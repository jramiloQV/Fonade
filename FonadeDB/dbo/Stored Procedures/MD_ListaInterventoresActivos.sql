CREATE PROCEDURE [dbo].[MD_ListaInterventoresActivos]
	-- Add the parameters for the stored procedure here
	@codGrupo int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--esta es la real 
--	SELECT DISTINCT c.Id_Contacto, c.Nombres + ' ' + c.Apellidos AS nombre, e.CodCoordinador 
--FROM Contacto c 
--INNER JOIN InterventorContrato ec ON c.Id_Contacto = ec.CodContacto 
--INNER JOIN Interventor e ON c.Id_Contacto = e.CodContacto 
--INNER JOIN GrupoContacto gc ON c.Id_Contacto = gc.CodContacto 
-- AND gc.CodGrupo =  @codGrupo
--WHERE (c.Inactivo = 0) 
--  AND (ec.FechaInicio < GETDATE()) 
-- AND (ec.FechaExpiracion > GETDATE())
		
	SELECT distinct Id_Contacto, Nombres + ' ' +Apellidos as nombre from Contacto c 
	inner join EvaluadorContrato ec on id_contacto = ec.codcontacto  
	inner join Evaluador e on id_contacto = e.codcontacto  
	inner join ProyectoContacto PC on id_contacto = pc.codcontacto  
	inner join grupocontacto gc on id_contacto = gc.codcontacto and codgrupo= @codGrupo
	WHERE c.inactivo = 0 
	order by nombre
END