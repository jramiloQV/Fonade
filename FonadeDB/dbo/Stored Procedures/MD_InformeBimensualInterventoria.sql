-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_InformeBimensualInterventoria]
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

	--IF @CodGrupo = @CONST_GerenteInterventor
		
	IF @CodGrupo = @CONST_CoordinadorInterventor
		BEGIN
			SELECT InformeBimensual.* FROM InformeBimensual
			INNER JOIN Interventor ON InformeBimensual.codinterventor = Interventor.CodContacto
			WHERE (Interventor.CodCoordinador = @CodUsuario)
			ORDER BY InformeBimensual.periodo
		END
	IF @CodGrupo = @CONST_GerenteInterventor 
	BEGIN
			SELECT InformeBimensual.* FROM InformeBimensual
			INNER JOIN Interventor ON InformeBimensual.codinterventor = Interventor.CodContacto
			ORDER BY InformeBimensual.periodo
		END
	
END