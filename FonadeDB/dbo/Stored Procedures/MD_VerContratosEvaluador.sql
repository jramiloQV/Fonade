
CREATE PROCEDURE [dbo].[MD_VerContratosEvaluador]

	@CodContacto int
	
AS

BEGIN

	SELECT 
	Id_EvaluadorContrato, 
	numContrato, 
	convert(varchar (50), FechaInicio, 107) as FechaInicio, 
	convert(varchar (50), FechaExpiracion, 107) as FechaExpiracion
	FROM EvaluadorContrato
	where CodContacto=@CodContacto

END