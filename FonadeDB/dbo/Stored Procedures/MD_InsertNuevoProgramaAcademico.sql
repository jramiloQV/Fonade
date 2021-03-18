
CREATE PROCEDURE MD_InsertNuevoProgramaAcademico
	@CodProgramaAcademico int,
	@nombreprograma varchar(350),
	@CodInstitucion int,
	@CodNivelEstudio int,
	@CiudadInstitucion int
AS

BEGIN
	INSERT INTO ProgramaAcademico 
	VALUES (@CodProgramaAcademico,@nombreprograma,'N/A',@CodInstitucion,'ACTIVO','N/A','','',@CodNivelEstudio,@CiudadInstitucion)
END