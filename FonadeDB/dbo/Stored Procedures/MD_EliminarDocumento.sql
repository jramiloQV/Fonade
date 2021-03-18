
CREATE PROCEDURE MD_EliminarDocumento
	@id_Archivo int
AS

BEGIN

	update Documento set borrado=1 where Id_Documento = @id_Archivo

END