CREATE PROCEDURE [dbo].[MD_InsertSectorEvaluador]

	@CodEvaluador int,
	@CodSector int
	
AS

BEGIN

	

	Insert into evaluadorsector (CodContacto,CodSector, Experiencia, fechaActualizacion)
	values(@CodEvaluador, @CodSector, 'A', GETDATE())

END