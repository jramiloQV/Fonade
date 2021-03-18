-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_CambiosPlanOperativo]
	-- Add the parameters for the stored procedure here
	@CodUsuario int,
	@CodGrupo int,
	@opcion VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	declare @CONST_CoordinadorInterventor int = 13
	declare @CONST_GerenteInterventor int = 12
	declare @CONST_RolInterventorLider int = 8

    -- Insert statements for procedure here
	if @CodGrupo = @CONST_CoordinadorInterventor
		BEGIN
			if @opcion = 'OPERATIVO'
				BEGIN
					SELECT
							ProyectoActividadPOInterventorTMP.Id_Actividad, ProyectoActividadPOInterventorTMP.NomActividad,
							ProyectoActividadPOInterventorTMP.CodProyecto, ProyectoActividadPOInterventorTMP.Item, ProyectoActividadPOInterventorTMP.Tarea,
							Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos
					FROM
							EmpresaInterventor
					INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa
					INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto
					INNER JOIN ProyectoActividadPOInterventorTMP ON Empresa.codproyecto = ProyectoActividadPOInterventorTMP.CodProyecto
					INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto
					WHERE
						(ProyectoActividadPOInterventorTMP.ChequeoGerente IS NULL)
						AND (ProyectoActividadPOInterventorTMP.ChequeoCoordinador IS NULL)
						AND (EmpresaInterventor.Rol = @CONST_RolInterventorLider)
						AND (EmpresaInterventor.Inactivo = 0)
						AND (Interventor.CodCoordinador = @CodUsuario)
				END
			if @opcion = 'NOMINA'
				BEGIN
					SELECT
							InterventorNominaTMP.Id_Nomina, InterventorNominaTMP.Cargo,
							InterventorNominaTMP.CodProyecto, InterventorNominaTMP.Tipo, InterventorNominaTMP.Tarea,
							Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos
					FROM
							EmpresaInterventor
					INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa
					INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto
					INNER JOIN InterventorNominaTMP ON Empresa.codproyecto = InterventorNominaTMP.CodProyecto
					INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto
					WHERE
							(InterventorNominaTMP.ChequeoGerente IS NULL)
							AND (InterventorNominaTMP.ChequeoCoordinador IS NULL)
							AND (EmpresaInterventor.Rol = @CONST_RolInterventorLider)
							AND (EmpresaInterventor.Inactivo = 0)
							AND (Interventor.CodCoordinador = @CodUsuario)
				END
			if @opcion = 'PRODUCCION'
				BEGIN
					SELECT
							InterventorProduccionTMP.Id_Produccion, InterventorProduccionTMP.NomProducto,
							InterventorProduccionTMP.CodProyecto, InterventorProduccionTMP.Tarea,
							Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos
					FROM
							EmpresaInterventor
					INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa
					INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto
					INNER JOIN InterventorProduccionTMP ON Empresa.codproyecto = InterventorProduccionTMP.CodProyecto
					INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto
					WHERE
							(InterventorProduccionTMP.ChequeoGerente IS NULL)
							AND (InterventorProduccionTMP.ChequeoCoordinador IS NULL)
							AND (EmpresaInterventor.Rol = @CONST_RolInterventorLider)
							AND (EmpresaInterventor.Inactivo = 0)
							AND (Interventor.CodCoordinador = @CodUsuario)
				END
			if @opcion = 'VENTAS'
				BEGIN
					SELECT
							InterventorVentasTMP.Id_Ventas, InterventorVentasTMP.NomProducto,
							InterventorVentasTMP.CodProyecto, InterventorVentasTMP.Tarea,
							Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos
					FROM
							EmpresaInterventor
					INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa
					INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto
					INNER JOIN InterventorVentasTMP ON Empresa.codproyecto = InterventorVentasTMP.CodProyecto
					INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto
					WHERE
							(InterventorVentasTMP.ChequeoGerente IS NULL)
							AND (InterventorVentasTMP.ChequeoCoordinador IS NULL)
							AND (EmpresaInterventor.Rol = @CONST_RolInterventorLider)
							AND (EmpresaInterventor.Inactivo = 0)
							AND (Interventor.CodCoordinador = @CodUsuario)
				END
		END
	if @CodGrupo = @CONST_GerenteInterventor
		begin
		if @opcion = 'OPERATIVO'
		begin
			SELECT ProyectoActividadPOInterventorTMP.Id_Actividad, ProyectoActividadPOInterventorTMP.NomActividad,
			ProyectoActividadPOInterventorTMP.CodProyecto, ProyectoActividadPOInterventorTMP.Item,
			ProyectoActividadPOInterventorTMP.Tarea, Empresa.razonsocial, Contacto.Nombres, Contacto.Apellidos
			FROM ProyectoActividadPOInterventorTMP
			INNER JOIN Empresa ON ProyectoActividadPOInterventorTMP.CodProyecto = Empresa.codproyecto
			INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa
			INNER JOIN Contacto ON EmpresaInterventor.CodContacto = Contacto.Id_Contacto
			WHERE (ProyectoActividadPOInterventorTMP.ChequeoGerente IS NULL)
			AND (ProyectoActividadPOInterventorTMP.ChequeoCoordinador = 1)
			AND (EmpresaInterventor.Inactivo = 0)
			AND (EmpresaInterventor.Rol = @CONST_RolInterventorLider)
		end
		end
END