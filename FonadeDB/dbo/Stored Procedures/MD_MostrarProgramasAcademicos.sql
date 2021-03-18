
CREATE PROCEDURE [dbo].[MD_MostrarProgramasAcademicos]
	@CodCiudad int,
	@CodNivel int,
	@nombreInst varchar(250),
	@programa varchar (350)
AS

BEGIN
	SELECT PA.Id_ProgramaAcademico, PA.NomProgramaAcademico
	FROM ProgramaAcademico PA 
	JOIN InstitucionEducativa IE ON (IE.Id_InstitucionEducativa = PA.CodInstitucionEducativa) 
	JOIN Ciudad C ON (C.Id_Ciudad= PA.CodCiudad)
	WHERE PA.CodNivelEstudio= @CodNivel and PA.CodCiudad=@CodCiudad and IE.NomInstitucionEducativa=@nombreInst
	and PA.NomProgramaAcademico LIKE '%' + @programa + '%'
END