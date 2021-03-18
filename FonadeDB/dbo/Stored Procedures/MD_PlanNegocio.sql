-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_PlanNegocio]
	-- Add the parameters for the stored procedure here
	@id_usuario int,	@Cod_grupo int,	@Cod_institucion int
AS
BEGIN

	Declare @GerenteAdministrador int = 1
	Declare @AdministradorFonade int = 2
	Declare @AdministradorSena int = 3
	Declare @JefeUnidad int = 4
	Declare @Asesor  int = 5
	Declare @Emprendedor int = 6
	Declare @CallCenter int = 8
	Declare @GerenteEvaluador int = 9
	Declare @CoordinadorEvaluador int = 10
	--Declare @Evaluador int = 11
	Declare @Evaluador int = 4 -- Mauricio Arias Olave. "09/05/2014": El valor estaba mal declarado.
	Declare @GerenteInterventor int = 12
	Declare @CoordinadorInterventor int = 13
	Declare @Interventor int = 14
	Declare @PerfilFiduciaria int = 15
	Declare @RolInterventorLider int = 8
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	IF @Cod_grupo  = @Asesor or @Cod_grupo = @Emprendedor
		BEGIN
			SELECT Id_Proyecto, NomProyecto FROM Proyecto WHERE Inactivo = 0
			 and CodInstitucion = @Cod_institucion and  exists (select codproyecto from proyectocontacto pc where id_proyecto=codproyecto and pc.codcontacto= @id_usuario and pc.inactivo=0)
		END
	IF @Cod_grupo  = @JefeUnidad
		BEGIN
			SELECT Id_Proyecto, NomProyecto FROM Proyecto WHERE Inactivo = 0
			 and CodInstitucion = @Cod_institucion
		END

	IF @Cod_grupo  = @GerenteEvaluador
		BEGIN
			SELECT Id_Proyecto, NomProyecto FROM Proyecto WHERE Inactivo = 0
			 and CodEstado = @Evaluador --Mauricio Arias Olave. "09/05/2014": Nuevas líneas.
			  ORDER BY NomProyecto
		END
	IF @Cod_grupo  = @Evaluador or @Cod_grupo = @CoordinadorEvaluador
		BEGIN
			SELECT Id_Proyecto, NomProyecto FROM Proyecto WHERE Inactivo = 0
			  and  exists (select codproyecto from proyectocontacto pc where id_proyecto=codproyecto and pc.codcontacto=@id_usuario and pc.inactivo=0)
		END
	IF @Cod_grupo  = @GerenteInterventor
		BEGIN
			SELECT Id_Proyecto, NomProyecto FROM Proyecto WHERE Inactivo = 0
			  and CodEstado IN (6,7)
		END

	IF @Cod_grupo  = @CoordinadorInterventor
		BEGIN
			SELECT Id_Proyecto, NomProyecto FROM Proyecto WHERE Inactivo = 0
			   and  exists (select codproyecto from proyectocontacto pc where id_proyecto=codproyecto and pc.codcontacto=@id_usuario and pc.inactivo=0)
		END

	IF @Cod_grupo  = @Interventor
		BEGIN
			SELECT Id_Proyecto, NomProyecto FROM Proyecto WHERE Inactivo = 0
			    and  Id_Proyecto IN (SELECT Empresa.codproyecto FROM Empresa INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa WHERE (EmpresaInterventor.Inactivo = 0) AND (EmpresaInterventor.CodContacto = @id_usuario))
		END
END