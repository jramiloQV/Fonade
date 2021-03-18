
CREATE PROCEDURE [dbo].[MD_CatalogoGestionInter]
	-- Add the parameters for the stored procedure here
	@CodUsuario int,
	@CodGrupo int
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @CONST_RolInterventorLider int = 8
	declare @CONST_CoordinadorInterventor int = 12
	
	IF @CodGrupo = @CONST_CoordinadorInterventor
		BEGIN
	

			SELECT InterventorIndicadorTMP.id_indicadorinter, InterventorIndicadorTMP.Aspecto,
			 InterventorIndicadorTMP.CodProyecto, InterventorIndicadorTMP.Tarea, 
			 Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos 
			FROM InterventorIndicadorTMP 
			INNER JOIN Empresa ON InterventorIndicadorTMP.CodProyecto = Empresa.codproyecto 
			 INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa 
			INNER JOIN Contacto ON EmpresaInterventor.CodContacto = Contacto.Id_Contacto 
			WHERE (InterventorIndicadorTMP.ChequeoGerente IS NULL)
			AND (InterventorIndicadorTMP.ChequeoCoordinador = 1)
			 AND (EmpresaInterventor.Inactivo = 0) 
			 AND (EmpresaInterventor.Rol =@CONST_RolInterventorLider)
			
			select * from Rol

		END
END