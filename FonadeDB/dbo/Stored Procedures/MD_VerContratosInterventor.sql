
CREATE PROCEDURE [dbo].[MD_VerContratosInterventor]

	@CodContacto int
	
AS

BEGIN

	SELECT 
	Id_InterventorContrato, 
	numContrato, 
	convert(varchar (50), FechaInicio, 107) as FechaInicio, 
	convert(varchar (50), FechaExpiracion, 107) as FechaExpiracion
	FROM InterventorContrato
	where CodContacto=@CodContacto

END