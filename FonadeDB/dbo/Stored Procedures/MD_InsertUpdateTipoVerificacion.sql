
CREATE PROCEDURE [dbo].[MD_InsertUpdateTipoVerificacion]

	@caso varchar(10)
	,@IdTipoCriterio int
	,@IDTipoInforme int
	,@nomTipoCriterio varchar(100)
	
AS
BEGIN
	BEGIN

	IF @caso='Create'

		begin

			Insert into InterventorInformeFinalItem
			(NomInterventorInformeFinalItem,CodInformeFinalCriterio)
			values (@nomTipoCriterio,@IDTipoInforme)


		

		end

	IF @caso='Update'

		begin

			Update InterventorInformeFinalItem set NomInterventorInformeFinalItem = @nomTipoCriterio , CodInformeFinalCriterio =@IDTipoInforme 

			WHERE Id_InterventorInformeFinalItem = @IdTipoInforme

			end

        IF @caso='Delete'

		begin

		DELETE FROM InterventorInformeFinalItem WHERE Id_InterventorInformeFinalItem = @IdTipoInforme
		end
			
END
END