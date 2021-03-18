CREATE PROCEDURE [dbo].[MD_AgendarTareas_Prueba]
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
Declare @AsesorLider int = 1

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
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

			select DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, '.' AS NombreRol 
			FROM Contacto c 
			join GrupoContacto on Id_Contacto=GrupoContacto.codcontacto and c.Inactivo =0
			and ((CodGrupo=@AdministradorFonade or CodGrupo=@AdministradorSena or codgrupo=@PerfilFiduciaria) or Id_Contacto=@id_usuario)
			order by Nombre
			OPTION (MAXRECURSION 2);
			
		END

		IF  @Cod_grupo  = @AdministradorFonade OR @Cod_grupo = @AdministradorSena
		BEGIN
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

			SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, '.' AS NombreRol FROM Contacto c 
			join GrupoContacto on id_contacto=codcontacto 
			and (codgrupo=@JefeUnidad or codgrupo=@AdministradorFonade or codgrupo=@PerfilFiduciaria or codgrupo=@AdministradorSena)
			order by Nombre
			OPTION (MAXRECURSION 2);
		END
	
		IF @Cod_grupo  = @JefeUnidad
		BEGIN
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

			SELECT distinct Id_Contacto, Nombres +' '+ Apellidos  + ' (.)' as Nombre, '(.)' AS NombreRol FROM Contacto c   
			join GrupoContacto on id_contacto=codcontacto and c.inactivo=0
			and codgrupo=@Asesor
			and CodInstitucion=@Cod_institucion
			or Id_Contacto=@id_usuario
			
			UNION ALL
			SELECT Id_Contacto, Nombres +' '+ Apellidos  + ' (Coordinador Interventor)' as Nombre,'Coordinador Interventor' AS NombreRol
			FROM contacto,grupocontacto 
			where id_contacto=codcontacto 
			and codgrupo=@CoordinadorInterventor  
			ORDER BY Nombre
			
			OPTION (MERGE UNION);
			
		END

		IF @Cod_grupo = @Emprendedor
		BEGIN

			DECLARE @estadoProyecto int
			SELECT @estadoProyecto=CodEstado FROM Proyecto INNER JOIN ProyectoContacto pc ON Id_Proyecto = CodProyecto WHERE PC.Inactivo=0 AND CodRol =3 AND pc.CodContacto = @id_usuario

			if (@estadoProyecto = 6 OR @estadoProyecto = 7)
			begin
			SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

				SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos  + ' (' +  Rol.Nombre + ')' as Nombre, Rol.Nombre AS NombreRol FROM Contacto c 
				join GrupoContacto on id_contacto=codcontacto and c.Inactivo=0
				join proyectocontacto pc on pc.inactivo=0 and pc.codcontacto=id_contacto and pc.codrol in (1,2,3)
				join proyectocontacto pc2 on pc2.inactivo=0  and pc2.codcontacto=@id_usuario
				and pc.codproyecto=pc2.codproyecto
				inner join Rol on pc.codrol=Id_Rol
				

				UNION ALL

				SELECT Id_Contacto, Nombres +' '+ Apellidos  + ' (' +  Rol.Nombre + ')' as Nombre, Rol.Nombre AS NombreRol
				FROM ProyectoContacto 
				INNER JOIN Contacto 
				INNER JOIN EmpresaInterventor ON Contacto.Id_Contacto = EmpresaInterventor.CodContacto and EmpresaInterventor.Inactivo=0
				INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa 
				INNER JOIN Rol ON EmpresaInterventor.Rol = Rol.Id_Rol ON ProyectoContacto.CodProyecto = dbo.Empresa.codproyecto 
				AND ProyectoContacto.Inactivo = 0 AND ProyectoContacto.CodContacto = @id_usuario

				ORDER BY Nombre
			end
			else
			SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

				SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos  + ' (' +  Rol.Nombre + ')' as Nombre, Rol.Nombre AS NombreRol FROM Contacto c 
				join GrupoContacto on id_contacto=codcontacto and c.Inactivo=0
				join proyectocontacto pc on pc.inactivo=0 and pc.codcontacto=id_contacto and pc.codrol in (1,2,3)
				join proyectocontacto pc2 on pc2.inactivo=0  and pc2.codcontacto=@id_usuario
				and pc.codproyecto=pc2.codproyecto
				inner join Rol on pc.codrol=Id_Rol
				
				ORDER BY Nombre
				OPTION (MERGE UNION);
		END

		IF @Cod_grupo = @Asesor
		BEGIN
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

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

			--SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos  + ' (' +  Rol.Nombre + ')' as Nombre, Rol.Nombre AS NombreRol FROM Contacto c 
			--join GrupoContacto on id_contacto=codcontacto and c.Inactivo=0
			--join proyectocontacto pc on pc.inactivo=0 and pc.codcontacto=id_contacto and pc.codrol in (1,2,3)
			--join proyectocontacto pc2 on pc2.inactivo=0  and pc2.codcontacto=@id_usuario
			--and pc.codproyecto=pc2.codproyecto
			--inner join Rol on pc.codrol=Id_Rol
			

			--UNION ALL

			--SELECT Id_Contacto, Nombres +' '+ Apellidos  + ' (' +  Rol.Nombre + ')' as Nombre, Rol.Nombre AS NombreRol 
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
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

			SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, '.' AS NombreRol FROM Contacto c
			join GrupoContacto on id_contacto=codcontacto and  codgrupo=@Evaluador and c.inactivo=0
			left join Evaluador e on id_contacto=e.codcontacto
			join EvaluadorContrato ec on id_contacto=ec.codcontacto
			union ALL
			SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, '.' as NombreRol FROM Contacto c 
			join GrupoContacto on id_contacto=codcontacto and  (codgrupo=@CoordinadorEvaluador or codgrupo=@PerfilFiduciaria or Id_Contacto=@id_usuario)
			 ORDER BY Nombre	
			OPTION (MERGE UNION);
		END
	
		IF @Cod_grupo = @CoordinadorEvaluador
		BEGIN
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

			SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, '.' AS NombreRol FROM Contacto c
			join GrupoContacto on id_contacto=codcontacto
			and codgrupo=@Evaluador
			join evaluador e on id_contacto=e.codcontacto
			join EvaluadorContrato ec on id_contacto=ec.codcontacto
			and e.codcoordinador = @id_usuario
			order by Nombre
			OPTION (MAXRECURSION 2);
		END

		IF @Cod_grupo =  @Evaluador
		BEGIN
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

			SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, '.' AS NombreRol FROM Contacto c
			join GrupoContacto on id_contacto=codcontacto and c.inactivo=0
			
			join proyectocontacto pc on pc.inactivo=0 and pc.codcontacto=id_contacto
			join proyectocontacto pc2 on pc2.inactivo=0 and pc2.codcontacto=@id_usuario
			and pc.codproyecto=pc2.codproyecto 
			
			UNION ALL
			SELECT DISTINCT c.Id_Contacto, c.Nombres +' '+ c.Apellidos as Nombre,'.' AS NombreRol 
			FROM Contacto c 
			join proyectocontacto pc on pc.inactivo=0 and pc.codcontacto=@id_usuario and c.inactivo=0
			join proyecto p on id_proyecto=pc.codproyecto
			join institucioncontacto ic on ic.codinstitucion = p.codinstitucion and ic.codcontacto=c.id_contacto
			and ic.fechafin is null
			ORDER BY Nombre
			OPTION (MERGE UNION);
		END

		IF @Cod_grupo = @GerenteInterventor
		BEGIN
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
			SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRol
			 FROM Contacto
			 INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto
			 INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo and  Grupo.Id_Grupo = @GerenteAdministrador
			  
			 UNION ALL
			 SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRol
			 FROM Contacto
			 INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto
			 INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo and Grupo.Id_Grupo = @GerenteEvaluador
			      
			 UNION ALL
			 SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRol
			 FROM Contacto 
			 INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto 
			 INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo and Grupo.Id_Grupo = @CoordinadorInterventor
			 
			 UNION ALL
			 SELECT distinct Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Rol.Nombre AS NombreRol
			 FROM Contacto
			 INNER JOIN Interventor ON Contacto.Id_Contacto = Interventor.CodContacto
			 INNER JOIN EmpresaInterventor ON Interventor.CodContacto =EmpresaInterventor.CodContacto
			 INNER JOIN Rol ON EmpresaInterventor.Rol = Rol.Id_Rol and Rol.Id_Rol = @RolInterventorLider
			
			 UNION ALL
			 SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRo
			 FROM Contacto			 INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto
			 INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo and Grupo.Id_Grupo = @PerfilFiduciaria
			     
			 ORDER BY NombreRol, Nombre
			
			OPTION (MERGE UNION);

		END

		IF @Cod_grupo = @CoordinadorInterventor
		BEGIN
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

			SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRol
			FROM Contacto
			INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto
			INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo and Grupo.Id_Grupo = @GerenteInterventor
			
			UNION ALL
			SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRol
			FROM Contacto
			INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto
			INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo and Grupo.Id_Grupo = @PerfilFiduciaria
			     
			UNION ALL
			SELECT DISTINCT Contacto.Id_Contacto, Contacto.Nombres +' '+Contacto.Apellidos AS Nombre, Rol.Nombre AS NombreRol
			FROM Interventor
			INNER JOIN Contacto ON Interventor.CodContacto = dbo.Contacto.Id_Contacto and Interventor.CodCoordinador = @CodUsuario
			INNER JOIN EmpresaInterventor ON Interventor.CodContacto = EmpresaInterventor.CodContacto
			INNER JOIN Rol ON EmpresaInterventor.Rol = Rol.Id_Rol
			AND EmpresaInterventor.Inactivo = 0
			AND Rol.Id_Rol = @RolInterventorLider
			UNION ALL
			SELECT DISTINCT Contacto.Id_Contacto, Contacto.Nombres +' '+Contacto.Apellidos AS Nombre, Rol.Nombre AS NombreRol
			FROM Interventor
			INNER JOIN Contacto ON Interventor.CodContacto = dbo.Contacto.Id_Contacto and Interventor.CodCoordinador = @CodUsuario
			INNER JOIN EmpresaInterventor ON Interventor.CodContacto = EmpresaInterventor.CodContacto
			INNER JOIN Rol ON EmpresaInterventor.Rol = Rol.Id_Rol
			AND EmpresaInterventor.Inactivo = 0
			AND Rol.Id_Rol = @RolInterventor
			UNION ALL
			SELECT DISTINCT Contacto.Id_Contacto, Contacto.Nombres +' '+Contacto.Apellidos AS Nombre, 'Jefe de Unidad' AS NombreRol
			FROM contacto,institucioncontacto
			WHERE contacto.id_contacto =codcontacto and fechafin is null
			ORDER BY NombreRol, Nombre
			OPTION (MERGE UNION);
		END
		
		IF @Cod_grupo = @Interventor
		BEGIN
		/*Pedro Carreño - 20/11/2014 - ERROR INT-02 - En la lista de personas para agendar tarea, 
		no está saliendo el rol entre paréntesis y falta los emprendedores. Inicio*/
		SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos + ' (' +  Grupo.NomGrupo + ')' AS Nombre, Grupo.NomGrupo AS NombreRol
		FROM Contacto 
		INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto 
		INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo
		WHERE     Grupo.Id_Grupo = 12
		UNION  
		SELECT Contacto.Id_contacto, Contacto.Nombres +' '+ Contacto.Apellidos + ' (' +  Grupo.NomGrupo + ')' AS Nombre, Grupo.NomGrupo AS NombreRol 
		FROM Contacto 
		INNER JOIN Interventor ON Contacto.Id_Contacto = Interventor.CodCoordinador 
		INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto 
		INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo
		--  WHERE Interventor.CodContacto =  3280  Cambio realizado el 06 de abril 2015- la consulta estaba trayendo datos quemados de un usuario
		WHERE Interventor.CodContacto = @id_usuario
		UNION 
		SELECT DISTINCT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos + ' (' +  Rol.Nombre + ')' AS Nombre, Rol.Nombre AS NombreRol 
		FROM Interventor 
		INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto 
		INNER JOIN EmpresaInterventor ON Contacto.Id_Contacto = EmpresaInterventor.CodContacto 
		INNER JOIN Rol ON EmpresaInterventor.Rol = Rol.Id_Rol 
		-- WHERE Interventor.CodCoordinador = (SELECT CodCoordinador FROM Interventor WHERE CodContacto =  3280) 
		WHERE Interventor.CodCoordinador = (SELECT CodCoordinador FROM Interventor WHERE CodContacto = @id_usuario) 
		AND (Rol.Id_Rol = 8)
		UNION 
		SELECT DISTINCT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos + ' (' +  Rol.Nombre + ')' AS Nombre, Rol.Nombre AS NombreRol 
		FROM Contacto 
		INNER JOIN ProyectoContacto ON Contacto.Id_Contacto = ProyectoContacto.CodContacto 
		INNER JOIN Rol ON ProyectoContacto.CodRol = Rol.Id_Rol 
		INNER JOIN Empresa ON ProyectoContacto.CodProyecto = Empresa.codproyecto 
		INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa 
		WHERE ProyectoContacto.Inactivo = 0 
		AND Rol.Id_Rol = 6 
		AND EmpresaInterventor.Inactivo = 0 
		AND EmpresaInterventor.CodContacto = @id_usuario
		-- AND EmpresaInterventor.CodContacto =  3280  Cambio realizado el 06 de abril 2015- la consulta estaba trayendo datos quemados de un usuario
		UNION 
		SELECT DISTINCT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos + ' (' +  Rol.Nombre + ')' AS Nombre, Rol.Nombre AS NombreRol 
		FROM Contacto 
		INNER JOIN ProyectoContacto ON Contacto.Id_Contacto = ProyectoContacto.CodContacto 
		INNER JOIN Rol ON ProyectoContacto.CodRol = Rol.Id_Rol 
		INNER JOIN Empresa ON ProyectoContacto.CodProyecto = Empresa.codproyecto 
		INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa 
		WHERE ProyectoContacto.Inactivo = 0 
		AND Rol.Id_Rol =1
		AND EmpresaInterventor.Inactivo = 0 
		--AND EmpresaInterventor.CodContacto =  3280 -- Cambio realizado el 06 de abril 2015- la consulta estaba trayendo datos quemados de un usuario
		AND EmpresaInterventor.CodContacto = @id_usuario 
		union
		SELECT DISTINCT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos + '( '+ Rol.Nombre + ' )' AS Nombre, Rol.Nombre AS NombreRol 
		FROM Contacto 
		INNER JOIN ProyectoContacto ON Contacto.Id_Contacto = ProyectoContacto.CodContacto 
		INNER JOIN Rol ON ProyectoContacto.CodRol = Rol.Id_Rol 
		INNER JOIN Empresa ON ProyectoContacto.CodProyecto = Empresa.codproyecto 
		INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa 
		WHERE ProyectoContacto.Inactivo = 0 
		AND Rol.Id_Rol = 3 
		AND EmpresaInterventor.Inactivo = 0 
		-- AND EmpresaInterventor.CodContacto = 3280 -- Cambio realizado el 06 de abril 2015- la consulta estaba trayendo datos quemados de un usuario
		AND EmpresaInterventor.CodContacto = @id_usuario
		ORDER BY NombreRol, Nombre	
		/*Pedro Carreño - 20/11/2014 - ERROR INT-02 - En la lista de personas para agendar tarea, 
		no está saliendo el rol entre paréntesis y falta los emprendedores. Inicio*/										  
END

IF @Cod_grupo = @PerfilFiduciaria
		BEGIN
		SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRol
													 FROM Contacto 
													 INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto 
													 INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo 
													 WHERE     Grupo.Id_Grupo =  @GerenteAdministrador
													 UNION 
													 SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRol 
													 FROM Contacto 
													 INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto 
													 INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo  
													 WHERE     Grupo.Id_Grupo = @CoordinadorInterventor
													 UNION 
													 SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRol 
													 FROM Contacto 
													 INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto 
													 INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo 
													 WHERE     Grupo.Id_Grupo = @AdministradorFonade
													 UNION 
													 SELECT Contacto.Id_Contacto, Contacto.Nombres +' '+ Contacto.Apellidos AS Nombre, Grupo.NomGrupo AS NombreRol 
													 FROM Contacto 
													 INNER JOIN GrupoContacto ON Contacto.Id_Contacto = GrupoContacto.CodContacto 
													 INNER JOIN Grupo ON GrupoContacto.CodGrupo = Grupo.Id_Grupo 
													 WHERE     Grupo.Id_Grupo =  @GerenteInterventor
													 ORDER BY NombreRol, Nombre

													 END
		END

			

END