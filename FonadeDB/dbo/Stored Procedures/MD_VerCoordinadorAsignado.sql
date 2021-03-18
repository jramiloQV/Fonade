
CREATE PROCEDURE [dbo].[MD_VerCoordinadorAsignado]
	@ContactoEval int
AS

BEGIN
	SELECT C.Id_Contacto, C.Nombres + ' ' + C.Apellidos as Nombre, C.Email as Email 
	FROM Contacto C, Evaluador
	WHERE C.Id_contacto = CodCoordinador
	AND codcontacto = @ContactoEval
END