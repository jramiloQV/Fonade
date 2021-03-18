-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_InformeEjecucionInterventoria]
	-- Add the parameters for the stored procedure here
	@CodGrupo INT,
	@CodUsuario INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CONST_GerenteInterventor INT = 12
	DECLARE @CONST_CoordinadorInterventor INT = 13
	DECLARE @CONST_Interventor INT = 14 --New line
    -- Insert statements for procedure here
	

	--IF @CodGrupo = @CONST_GerenteInterventor
		
	IF @CodGrupo = @CONST_CoordinadorInterventor
		BEGIN
			SELECT InformePresupuestal.* FROM InformePresupuestal
			INNER JOIN Interventor ON InformePresupuestal.codinterventor = Interventor.CodContacto
			WHERE (Interventor.CodCoordinador = @CodUsuario)
			ORDER BY dbo.InformePresupuestal.NomInformePresupuestal
		END
	IF @CodGrupo = @CONST_GerenteInterventor
		BEGIN
			SELECT InformePresupuestal.* FROM InformePresupuestal
			INNER JOIN Interventor ON InformePresupuestal.codinterventor = Interventor.CodContacto
			--WHERE (Interventor.CodCoordinador = @CodUsuario)
			ORDER BY dbo.InformePresupuestal.NomInformePresupuestal
		END
		--Added at May 19, 2014.
    IF @CodGrupo = @CONST_Interventor
		BEGIN
			SELECT * FROM InformePresupuestal 
			WHERE codinterventor = @CodUsuario 
			--AND (UPPER (NomInformePresupuestal) LIKE '" & UCase(txtLetra) & "%') 
			ORDER BY NomInformePresupuestal --"&txtCampoOrden
		END
END