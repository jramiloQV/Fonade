

CREATE PROCEDURE [dbo].[MD_Insert_Update_Estudios]

	@CODCONTACTO int
	, @CODPROGRAMAACADEMICO int
	, @TITULOOBTENIDO varchar(500)
	, @ANOTITULO smallint = null
	, @INSTITUCION varchar(500) 
	, @CODCIUDAD int
	, @CODNIVELESTUDIO int
	, @FINALIZADO int
	, @FECHAINICIO datetime
	, @FECHAFINMATERIA datetime  = null
	, @FECHAGRADO datetime = null
	, @FECHAULTIMOCORTE datetime = null
	, @SEMESTRESCURSADOS int = null
	, @IDCONTACTOESTUDIO int = null
	, @caso varchar(10)
AS

BEGIN

	IF @caso='Create'

		begin

			INSERT INTO ContactoEstudio(CodContacto,CodProgramaAcademico, TituloObtenido, AnoTitulo, Institucion, CodCiudad, CodNivelEstudio,Finalizado,FechaInicio,FechaFinMaterias,FechaGrado,FechaUltimoCorte,SemestresCursados,FlagIngresadoAsesor,fechaCreacion) 
			VALUES ( @CODCONTACTO,@CODPROGRAMAACADEMICO, @TITULOOBTENIDO, @ANOTITULO, @INSTITUCION, @CODCIUDAD, @CODNIVELESTUDIO,@FINALIZADO,@FECHAINICIO,@FECHAFINMATERIA,@FECHAGRADO,@FECHAULTIMOCORTE,@SEMESTRESCURSADOS,0,GETDATE())

		end


	IF @caso='Update'

		begin

			UPDATE ContactoEstudio 
			SET TituloObtenido = @TITULOOBTENIDO
			, AnoTitulo = @ANOTITULO
			, Institucion = @INSTITUCION
			, CodCiudad=@CODCIUDAD
			, CodNivelEstudio=@CODNIVELESTUDIO
			, CodProgramaAcademico=@CODPROGRAMAACADEMICO
			, Finalizado = @FINALIZADO
			, FechaInicio = @FECHAINICIO
			, FechaFinMaterias = @FECHAFINMATERIA
			, FechaGrado=@FECHAGRADO
			, FechaUltimoCorte = @FECHAULTIMOCORTE
			, SemestresCursados = @SEMESTRESCURSADOS
			, fechaActualizacion=GETDATE() 
			WHERE Id_ContactoEstudio = @IDCONTACTOESTUDIO
		
		end
END