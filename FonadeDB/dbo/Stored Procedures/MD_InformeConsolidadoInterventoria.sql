-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[MD_InformeConsolidadoInterventoria]
	-- Add the parameters for the stored procedure here
	@CodGrupo INT,
	@CodUsuario INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @CONST_GerenteInterventor INT = 12
	DECLARE @CONST_CoordinadorInterventor INT = 13
	DECLARE @CONST_Interventor INT = 14 --New lines.

	--IF @CodGrupo = @CONST_GerenteInterventor
		
	IF @CodGrupo = @CONST_CoordinadorInterventor
		BEGIN
			SELECT InterventorInformeFinal.* FROM InterventorInformeFinal
			INNER JOIN Interventor ON InterventorInformeFinal.CodInterventor = Interventor.CodContacto
			WHERE (Interventor.CodCoordinador = @CodUsuario)
			ORDER BY InterventorInformeFinal.NomInterventorInformeFinal
		END
	IF @CodGrupo = @CONST_GerenteInterventor
	BEGIN
			SELECT InterventorInformeFinal.* FROM InterventorInformeFinal
			INNER JOIN Interventor ON InterventorInformeFinal.CodInterventor = Interventor.CodContacto
			--WHERE (Interventor.CodCoordinador = @CodUsuario)
			ORDER BY InterventorInformeFinal.NomInterventorInformeFinal
		END
	END
	-- Added at May 19, 2014.
	IF @CodGrupo = @CONST_Interventor
	BEGIN
		SELECT * FROM InterventorInformeFinal 
		WHERE codinterventor= @CodUsuario 
		--AND (UPPER (NomInterventorInformeFinal) like '" & UCase(txtLetra) & "%') 
		ORDER BY NomInterventorInformeFinal --"&txtCampoOrden
	END