
create PROCEDURE MD_EliminarBeneficiario
	@id_pagobenef int
AS

BEGIN

	DELETE FROM PagoBeneficiario WHERE Id_PagoBeneficiario = @id_pagobenef

END