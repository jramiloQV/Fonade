
CREATE PROCEDURE [dbo].[MD_MostrarInstituciones]
	@CodCiudad int,
	@CodNivel int
AS

BEGIN
	SELECT Distinct IE.NomInstitucionEducativa
	FROM ProgramaAcademico PA 
	JOIN InstitucionEducativa IE ON (IE.Id_InstitucionEducativa = PA.CodInstitucionEducativa) 
	JOIN Ciudad C ON (C.Id_Ciudad= PA.CodCiudad)
	WHERE PA.CodNivelEstudio= @CodNivel and PA.CodCiudad=@CodCiudad
	order by NomInstitucionEducativa
END