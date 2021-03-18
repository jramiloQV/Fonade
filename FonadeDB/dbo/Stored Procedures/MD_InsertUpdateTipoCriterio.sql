
CREATE PROCEDURE [dbo].[MD_InsertUpdateTipoCriterio]

	@caso varchar(10)
	,@IdTipoCriterio int
	,@nomTipoCriterio varchar(100)
	
AS
BEGIN
	BEGIN

	IF @caso='Create'

		begin

			Insert into InterventorInformeFinalCriterio
			(NomInterventorInformeFinalCriterio,CodEmpresa) 
			values (@nomTipoCriterio,0)


		

		end

	IF @caso='Update'

		begin

			Update InterventorInformeFinalCriterio set NomInterventorInformeFinalCriterio = @nomTipoCriterio

			WHERE Id_InterventorInformeFinalCriterio = @IdTipoCriterio

			end
END
END