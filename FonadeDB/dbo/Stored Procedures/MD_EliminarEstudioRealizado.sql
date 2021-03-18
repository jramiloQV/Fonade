CREATE PROCEDURE MD_EliminarEstudioRealizado
	@id_estudio int
AS

BEGIN

	DELETE FROM ContactoEstudio WHERE Id_ContactoEstudio = @id_estudio

END