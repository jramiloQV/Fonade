
CREATE PROCEDURE [dbo].[MD_MostrarIdInstitucionAct]
	@CodCiudad int,
	@CodNivel int,
	@institucion varchar(250)
AS

BEGIN
	SELECT top 1 IE.Id_InstitucionEducativa
	FROM ProgramaAcademico PA 
	JOIN InstitucionEducativa IE ON (IE.Id_InstitucionEducativa = PA.CodInstitucionEducativa) 
	JOIN Ciudad C ON (C.Id_Ciudad= PA.CodCiudad)
	WHERE PA.CodNivelEstudio= @CodNivel and PA.CodCiudad=@CodCiudad and IE.NomInstitucionEducativa=@institucion
END