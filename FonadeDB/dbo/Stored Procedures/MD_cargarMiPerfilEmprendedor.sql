
CREATE PROCEDURE [dbo].[MD_cargarMiPerfilEmprendedor]
	@id_contacto int
AS

BEGIN
	SELECT Identificacion, Nombres, Apellidos, Email, Genero, FechaNacimiento,isnull(CodCiudad, 111) AS CodCiudad, 
	Telefono, isnull(LugarExpedicionDI, 111) as LugarExpedicionDI 
	FROM Contacto 
	WHERE Id_Contacto = @id_contacto
END