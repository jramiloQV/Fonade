
CREATE PROCEDURE [dbo].[MD_InformeVistaInterventoria]
	-- Add the parameters for the stored procedure here
	@CodGrupo INT,
	@CodUsuario INT
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @CONST_GerenteInterventor INT = 12
	DECLARE @CONST_CoordinadorInterventor INT = 13

	IF @CodGrupo = @CONST_GerenteInterventor
		BEGIN
			SELECT * FROM InformeVisitaInterventoria order by NombreInforme
		END
	IF @CodGrupo = @CONST_CoordinadorInterventor
		BEGIN
			SELECT InformeVisitaInterventoria.* FROM InformeVisitaInterventoria
			INNER JOIN Interventor ON InformeVisitaInterventoria.CodInterventor = Interventor.CodContacto
			WHERE (Interventor.CodCoordinador = @CodUsuario)
			ORDER BY InformeVisitaInterventoria.NombreInforme
		END
	IF @CodGrupo <> @CONST_GerenteInterventor AND @CodGrupo <>@CONST_CoordinadorInterventor
		BEGIN
			SELECT * FROM InformeVisitaInterventoria WHERE CodInterventor=@CodUsuario order by NombreInforme
		END
END