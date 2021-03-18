CREATE PROCEDURE [dbo].[MD_AgendarTareas]
	/**/
	@id_usuario int,	@Cod_grupo int,	@Cod_institucion int,	@accion varchar(255),
	/*Agendar Tarea*/
	@ParaQuien int,@Proyecto int,@NomTarea  varchar (255),@Descripcion text,@CodTareaPrograma int,@Recurrente varchar(3) ,@RecordatorioEmail bit,
	@NivelUrgencia smallint,@RecordatorioPantalla bit,@RequiereRespuesta bit,@CodUsuario int,@DocumentoRelacionado varchar(255)

		
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
Declare @Evaluador int = 11
Declare @GerenteInterventor int = 12
Declare @CoordinadorInterventor int = 13
Declare @Interventor int = 14
Declare @PerfilFiduciaria int = 15
Declare @RolInterventorLider int = 8
Declare @RolInterventor int = 6

IF  @accion  = 'Adicionar'
BEGIN

 INSERT INTO TareaUsuario
 (CodContacto,
  CodProyecto, 
  NomTareaUsuario,
  Descripcion, 
  CodTareaPrograma, 
  Recurrente, 
  RecordatorioEmail, 
  NivelUrgencia, 
  RecordatorioPantalla, 
  RequiereRespuesta, 
  CodContactoAgendo, 
  DocumentoRelacionado)
		VALUES
		(@ParaQuien,
		@Proyecto,
		@NomTarea,
		@Descripcion,
		@CodTareaPrograma,
		@Recurrente,
		@RecordatorioEmail,
		@NivelUrgencia,
		@RecordatorioPantalla,
		@RequiereRespuesta,
		@CodUsuario,
		@DocumentoRelacionado)
		END
IF  @accion  = 'TraerUsuarios'
BEGIN
		IF @Cod_grupo  = @GerenteAdministrador 
		BEGIN
			select DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, '.' AS NombreRol 
			FROM Contacto c 
			join GrupoContacto on Id_Contacto=GrupoContacto.codcontacto 
			and ((CodGrupo=@AdministradorFonade or CodGrupo=@AdministradorSena or codgrupo=@PerfilFiduciaria) or Id_Contacto=@id_usuario)
			where c.Inactivo=0 order by Nombre
		END

		IF  @Cod_grupo  = @AdministradorFonade OR @Cod_grupo = @AdministradorSena
		BEGIN
			SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, '.' AS NombreRol FROM Contacto c 
			join GrupoContacto on id_contacto=codcontacto 
			and (codgrupo=@JefeUnidad or codgrupo=@AdministradorFonade or codgrupo=@PerfilFiduciaria or codgrupo=@AdministradorSena)
		END
	
		IF @Cod_grupo  = @JefeUnidad
		BEGIN
			SELECT distinct Id_Contacto, Nombres +' '+ Apellidos  + ' (.)' as Nombre, '(.)' AS NombreRol FROM Contacto c   
			join GrupoContacto on id_contacto=codcontacto 
			and codgrupo=@Asesor
			and CodInstitucion=@Cod_institucion
			or Id_Contacto=@id_usuario
			where c.inactivo=0
			UNION
			SELECT Id_Contacto, Nombres +' '+ Apellidos  + ' (Coordinador Interventor)' as Nombre,'Coordinador Interventor' AS NombreRol
			FROM contacto,grupocontacto 
			where id_contacto=codcontacto 
			and codgrupo=@CoordinadorInterventor  --
			ORDER BY Nombre
		END

		IF @Cod_grupo = @Emprendedor
		BEGIN

			DECLARE @estadoProyecto int
			SELECT @estadoProyecto=CodEstado FROM Proyecto INNER JOIN ProyectoContacto pc ON Id_Proyecto = CodProyecto WHERE PC.Inactivo=0 AND CodRol =3 AND pc.CodContacto = @id_usuario

			if @estadoProyecto = 6
			begin
				SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos  + ' (' +  Rol.Nombre + ')' as Nombre, Rol.Nombre AS NombreRol FROM Contacto c 
				join GrupoContacto on id_contacto=codcontacto
				join proyectocontacto pc on pc.inactivo=0 and pc.codcontacto=id_contacto and pc.codrol in (1,2,3)
				join proyectocontacto pc2 on pc2.inactivo=0  and pc2.codcontacto=@id_usuario
				and pc.codproyecto=pc2.codproyecto
				inner join Rol on pc.codrol=Id_Rol
				where c.Inactivo=0

				UNION

				SELECT Id_Contacto, Nombres +' '+ Apellidos  + ' (' +  Rol.Nombre + ')' as Nombre, Rol.Nombre AS NombreRol
				FROM ProyectoContacto 
				INNER JOIN Contacto 
				INNER JOIN EmpresaInterventor ON Contacto.Id_Contacto = EmpresaInterventor.CodContacto 
				INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa 
				INNER JOIN Rol ON EmpresaInterventor.Rol = Rol.Id_Rol ON ProyectoContacto.CodProyecto = dbo.Empresa.codproyecto 
				WHERE EmpresaInterventor.Inactivo = 0 
				AND ProyectoContacto.Inactivo = 0 AND ProyectoContacto.CodContacto = @id_usuario

				ORDER BY Nombre
			end
			else
				SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos  + ' (' +  Rol.Nombre + ')' as Nombre, Rol.Nombre AS NombreRol FROM Contacto c 
				join GrupoContacto on id_contacto=codcontacto
				join proyectocontacto pc on pc.inactivo=0 and pc.codcontacto=id_contacto and pc.codrol in (1,2,3)
				join proyectocontacto pc2 on pc2.inactivo=0  and pc2.codcontacto=@id_usuario
				and pc.codproyecto=pc2.codproyecto
				inner join Rol on pc.codrol=Id_Rol
				where c.Inactivo=0
				ORDER BY Nombre
		END

		IF @Cod_grupo = @Asesor
		BEGIN

			SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, Rol.Nombre AS NombreRol FROM Contacto c 
			join GrupoContacto on id_contacto=codcontacto and c.Inactivo=0
			join proyectocontacto pc on pc.inactivo=0 and pc.codcontacto=id_contacto and pc.codrol in (1,2,3)
			join proyectocontacto pc2 on pc2.inactivo=0  and pc2.codcontacto=@id_usuario
			and pc.codproyecto=pc2.codproyecto
			inner join Rol on pc.codrol=Id_Rol
			

			UNION ALL

			SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, Rol.Nombre AS NombreRol 
			FROM ProyectoContacto 
			INNER JOIN Contacto 
			INNER JOIN EmpresaInterventor ON Contacto.Id_Contacto = EmpresaInterventor.CodContacto and EmpresaInterventor.Inactivo=0
			INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa 
			INNER JOIN Rol ON EmpresaInterventor.Rol = Rol.Id_Rol ON ProyectoContacto.CodProyecto = dbo.Empresa.codproyecto 
			
			AND ProyectoContacto.Inactivo = 0 AND ProyectoContacto.CodContacto = @id_usuario

			ORDER BY Nombre
			OPTION (MERGE UNION);

			--SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, Rol.Nombre AS NombreRol FROM Contacto c 
			--join GrupoContacto on id_contacto=codcontacto and c.Inactivo=0
			--join proyectocontacto pc on pc.inactivo=0 and pc.codcontacto=id_contacto and pc.codrol in (1,2,3)
			--join proyectocontacto pc2 on pc2.inactivo=0  and pc2.codcontacto=@id_usuario
			--and pc.codproyecto=pc2.codproyecto
			--inner join Rol on pc.codrol=Id_Rol
			

			--UNION ALL

			--SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, Rol.Nombre AS NombreRol 
			--FROM ProyectoContacto 
			--INNER JOIN Contacto 
			--INNER JOIN EmpresaInterventor ON Contacto.Id_Contacto = EmpresaInterventor.CodContacto and EmpresaInterventor.Inactivo=0
			--INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa 
			--INNER JOIN Rol ON EmpresaInterventor.Rol = Rol.Id_Rol ON ProyectoContacto.CodProyecto = dbo.Empresa.codproyecto 
			
			--AND ProyectoContacto.Inactivo = 0 AND ProyectoContacto.CodContacto = @id_usuario

			--ORDER BY Nombre
			--OPTION (MERGE UNION);
			
		END
	
		IF @Cod_grupo  = @GerenteEvaluador
		BEGIN
			SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, '.' AS NombreRol FROM Contacto c
			join GrupoContacto on id_contacto=codcontacto and  codgrupo=@Evaluador
			left join Evaluador e on id_contacto=e.codcontacto
			join EvaluadorContrato ec on id_contacto=ec.codcontacto
			union
			SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, '.' as NombreRol FROM Contacto c 
			join GrupoContacto on id_contacto=codcontacto and  (codgrupo=@CoordinadorEvaluador or codgrupo=@PerfilFiduciaria or Id_Contacto=@id_usuario)
			where c.inactivo=0 ORDER BY Nombre	
		END
	
		IF @Cod_grupo = @CoordinadorEvaluador
		BEGIN
			SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, '.' AS NombreRol FROM Contacto c
			join GrupoContacto on id_contacto=codcontacto
			and codgrupo=@Evaluador
			join evaluador e on id_contacto=e.codcontacto
			join EvaluadorContrato ec on id_contacto=ec.codcontacto
			and e.codcoordinador = @id_usuario
		END

		IF @Cod_grupo =  @Evaluador
		BEGIN
			SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, '.' AS NombreRol FROM Contacto c
			join GrupoContacto on id_contacto=codcontacto
			
			join proyectocontacto pc on pc.inactivo=0 and pc.codcontacto=id_contacto
			join proyectocontacto pc2 on pc2.inactivo=0 and pc2.codcontacto=@id_usuario
			and pc.codproyecto=pc2.codproyecto 
			where c.inactivo=0 
			UNION
			SELECT DISTINCT c.Id_Contacto, c.Nombres +' '+ c.Apellidos as Nombre,'.' AS NombreRol 
			FROM Contacto c 
			join proyectocontacto pc on pc.inactivo=0 and pc.codcontacto=@id_usuario
			join proyecto p on id_proyecto=pc.codproyecto
			join institucioncontacto ic on ic.codinstitucion = p.codinstitucion and ic.codcontacto=c.id_contacto
			and ic.fechafin is null
			where c.inactivo=0 ORDER BY Nombre
		END

		IF @Cod_grupo = @GerenteInterventor
		BEGIN
			SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRol
			 FROM Contacto
			 INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto
			 INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo
			 WHERE     Grupo.Id_Grupo = @GerenteAdministrador
			 UNION
			 SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRol
			 FROM Contacto
			 INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto
			 INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo
			 WHERE     Grupo.Id_Grupo = @GerenteEvaluador
			 UNION 
			 SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRol
			 FROM Contacto 
			 INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto 
			 INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo
			 WHERE     Grupo.Id_Grupo = @CoordinadorInterventor
			 UNION 
			 SELECT distinct Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Rol.Nombre AS NombreRol
			 FROM Contacto
			 INNER JOIN Interventor ON Contacto.Id_Contacto = Interventor.CodContacto
			 INNER JOIN EmpresaInterventor ON Interventor.CodContacto =EmpresaInterventor.CodContacto
			 INNER JOIN Rol ON EmpresaInterventor.Rol = Rol.Id_Rol
			 WHERE Rol.Id_Rol = @RolInterventorLider
			 UNION
			 SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRo
			 FROM Contacto			 INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto
			 INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo
			 WHERE     Grupo.Id_Grupo = @PerfilFiduciaria
			 ORDER BY NombreRol, Nombre
			


		END

		IF @Cod_grupo = @CoordinadorInterventor
		BEGIN
			SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRol
			FROM Contacto
			INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto
			INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo
			WHERE     Grupo.Id_Grupo = @GerenteInterventor
			UNION
			SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRol
			FROM Contacto
			INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto
			INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo
			WHERE     Grupo.Id_Grupo = @PerfilFiduciaria
			UNION
			SELECT DISTINCT Contacto.Id_Contacto, Contacto.Nombres +' '+Contacto.Apellidos AS Nombre, Rol.Nombre AS NombreRol
			FROM Interventor
			INNER JOIN Contacto ON Interventor.CodContacto = dbo.Contacto.Id_Contacto
			INNER JOIN EmpresaInterventor ON Interventor.CodContacto = EmpresaInterventor.CodContacto
			INNER JOIN Rol ON EmpresaInterventor.Rol = Rol.Id_Rol
			WHERE Interventor.CodCoordinador = @CodUsuario
			AND EmpresaInterventor.Inactivo = 0
			AND Rol.Id_Rol = @RolInterventorLider
			UNION
			SELECT DISTINCT Contacto.Id_Contacto, Contacto.Nombres +' '+Contacto.Apellidos AS Nombre, Rol.Nombre AS NombreRol
			FROM Interventor
			INNER JOIN Contacto ON Interventor.CodContacto = dbo.Contacto.Id_Contacto
			INNER JOIN EmpresaInterventor ON Interventor.CodContacto = EmpresaInterventor.CodContacto
			INNER JOIN Rol ON EmpresaInterventor.Rol = Rol.Id_Rol
			WHERE Interventor.CodCoordinador = @CodUsuario
			AND EmpresaInterventor.Inactivo = 0
			AND Rol.Id_Rol = @RolInterventor
			UNION
			SELECT DISTINCT Contacto.Id_Contacto, Contacto.Nombres +' '+Contacto.Apellidos AS Nombre, 'Jefe de Unidad' AS NombreRol
			FROM contacto,institucioncontacto
			WHERE contacto.id_contacto =codcontacto and fechafin is null
			ORDER BY NombreRol, Nombre
		END
		END

			

END