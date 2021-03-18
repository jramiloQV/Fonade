
CREATE PROCEDURE [dbo].[MD_ReporteEmpresa]
	-- Add the parameters for the stored procedure here
	@CodGrupo INT,
	@CodUsuario INT,
	@CodInforme INT
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @CONST_Interventor INT = 14
	DECLARE @CONST_GerenteInterventor INT = 12
	DECLARE @CONST_CoordinadorInterventor INT = 13
	
	DECLARE @CodEmpresa INT = 0

	--IF @CodGrupo = @CONST_GerenteInterventor
		
	IF @CodGrupo = @CONST_CoordinadorInterventor
		BEGIN
			
			IF @CodInforme != 0
				begin
					Select @CodEmpresa = CodEmpresa from informevisitainterventoria where id_informe = @CodInforme
				
					IF @CodEmpresa != 0
						BEGIN
							SELECT DISTINCT e.id_empresa, e.razonsocial, e.codproyecto, e.CodCiudad, e.nit
							FROM Empresa e, Proyecto p, EmpresaInterventor ei
							WHERE e.codproyecto = p.Id_Proyecto
							AND e.id_empresa = ei.CodEmpresa AND e.id_empresa = @CodEmpresa ORDER BY e.razonsocial
						END
				end
			ELSE
				BEGIN
					SELECT DISTINCT e.id_empresa, e.razonsocial, e.codproyecto, e.CodCiudad, e.nit
					FROM Empresa e, Proyecto p, EmpresaInterventor ei
					WHERE e.codproyecto = p.Id_Proyecto
					AND e.id_empresa = ei.CodEmpresa ORDER BY e.razonsocial
				END
		END

	-- New Lines added at May 19, 2014.
	IF @CodGrupo = @CONST_Interventor
		BEGIN
			--SELECT id_empresa, razonsocial, nit 
			--FROM Empresa WHERE (id_empresa IN (SELECT CodEmpresa FROM EmpresaInterventor 
			--								   WHERE CodContacto = @CodUsuario)) 
			--ORDER BY razonsocial
			--SELECT DISTINCT e.id_empresa, e.razonsocial, e.codproyecto, e.CodCiudad, e.nit
			--FROM Empresa e, Proyecto p, EmpresaInterventor ei
			--WHERE e.codproyecto = p.Id_Proyecto
			--AND e.id_empresa = ei.CodEmpresa AND ei.CodContacto = @CodUsuario
			Select * from InformeVisitaInterventoria i, Empresa e where i.CodEmpresa = e.Id_Empresa and id_informe = @CodInforme
		END
	--IF @CodGrupo <> @CONST_GerenteInterventor AND @CodGrupo <>@CONST_CoordinadorInterventor
		
END