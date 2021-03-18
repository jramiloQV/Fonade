create PROCEDURE [dbo].[MD_DeleteSectorEvaluador]

	@CodEvaluador int
	
AS

BEGIN

	delete from evaluadorsector where CodContacto=@CodEvaluador AND Experiencia='A'

END