-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE UsuariosPorProyecto
	-- Add the parameters for the stored procedure here
	@IdProyecto as int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;		
		Select
			*
		from 
			(
				Select 
					Proyecto.Id_Proyecto CodigoProyecto, 
					Proyecto.NomProyecto NombreProyecto,
					Estado.NomEstado EstadoProyecto,
					Contacto.Id_Contacto CodigoContacto,
					Contacto.Nombres + ' ' +Contacto.Apellidos NombresContacto,
					Contacto.Email EmailContacto,
					Contacto.Clave ClaveContacto,
					CASE WHEN Contacto.Inactivo = 0 then 'Activo' else 'Desactivado' END AS EstadoContacto,
					Grupo.NomGrupo GrupoContacto,
					Rol.Nombre RolContacto
				from 
					EmpresaInterventor
					Inner Join Empresa  on Empresa.id_empresa = EmpresaInterventor.CodEmpresa
					Inner Join Contacto on Contacto.Id_Contacto = EmpresaInterventor.CodContacto
					Inner join Proyecto on Proyecto.Id_Proyecto = Empresa.codproyecto
					inner join Estado on proyecto.CodEstado = estado.Id_Estado
					Inner join rol on rol.Id_Rol = EmpresaInterventor.Rol
					inner join GrupoContacto on Contacto.Id_Contacto = GrupoContacto.CodContacto
					inner join Grupo on GrupoContacto.CodGrupo = grupo.Id_Grupo
				where 
					Empresa.codproyecto = @IdProyecto
					and EmpresaInterventor.Inactivo = 0
				Union
					SELECT
					DISTINCT 
					Proyecto.Id_Proyecto CodigoProyecto, 
					Proyecto.NomProyecto NombreProyecto,
					Estado.NomEstado EstadoProyecto,
					Contacto.Id_Contacto CodigoContacto,
					Contacto.Nombres + ' ' +Contacto.Apellidos NombresContacto,
					Contacto.Email EmailContacto,
					Contacto.Clave ClaveContacto,
					CASE WHEN Contacto.Inactivo = 0 then 'Activo' else 'Desactivado' END AS EstadoContacto,
					Grupo.NomGrupo GrupoContacto,
					Rol.Nombre RolContacto
				FROM 
					proyecto 
					INNER JOIN estado ON estado.id_estado = proyecto.codestado 
					INNER JOIN proyectoContacto ON proyectocontacto.codproyecto = proyecto.id_proyecto and proyectoContacto.Inactivo = 0 
					INNER JOIN contacto ON contacto.id_contacto = proyectocontacto.codcontacto 
					INNER JOIN GrupoContacto on contacto.id_contacto = GrupoContacto.CodContacto 
					INNER JOIN grupo on grupo.id_grupo = GrupoContacto.codgrupo
					INNER JOIN Rol on ProyectoContacto.CodRol = Rol.Id_Rol
				WHERE
					proyecto.id_proyecto = @IdProyecto ) t 
					order by t.RolContacto
END