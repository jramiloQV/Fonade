
create PROCEDURE [dbo].[MD_ActivarEvaluadores]
	@CodContacto int
AS

BEGIN

	UPDATE Contacto 
	SET Inactivo=0 
	WHERE Id_Contacto=@CodContacto


	UPDATE ContactoDesactivacion 
	SET 
	FechaFin=getdate() 
	WHERE Id_ContactoDesactivacion=
	(
		SELECT MAX(Id_ContactoDesactivacion) FROM ContactoDesactivacion WHERE codContacto=@CodContacto
	)

END