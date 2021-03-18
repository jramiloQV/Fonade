
CREATE PROCEDURE [dbo].[MD_VerFormalizacionQuerys]
	@id_proyecto int,
	@CONST_RolAsesor int,
	@CONST_RolAsesorLider int,
	@CONST_RolEmprendedor int,
	@caso int
AS

BEGIN

	Declare @idproyectoformalizacion int
	Declare @codigoconvocatoria int
	SELECT @idproyectoformalizacion = Id_ProyectoFormalizacion, @codigoconvocatoria = CodConvocatoria 
	FROM ProyectoFormalizacion WHERE CodProyecto = @id_proyecto ORDER BY Id_ProyectoFormalizacion ASC
	
	-----------------------------------
	
	IF @caso=1

	begin

		SELECT id_proyecto as idproyectoQ1,nomProyecto as nomProyectoQ1, sumario as sumarioQ1, P.FechaCreacion AS FechaCreacionQ1, nomTipoProyecto AS nomTipoProyectoQ1, 
		nomEstado AS nomEstadoQ1, nomSubSector AS nomSubSectorQ1, nomSector AS nomSectorQ1, nomInstitucion AS nomInstitucionQ1, 
		nomUnidad AS nomUnidadQ1, J.nombres AS nombresQ1, J.apellidos AS apellidosQ1, J.Identificacion AS IdentificacionQ1, 
		nomTipoIdentificacion AS nomTipoIdentificacionQ1, nomCiudad + ' (' + nomDepartamento + ')' AS nomCiudadQ1, nomDepartamento AS nomDepartamentoQ1, 
		PlanNacional AS PlanNacionalQ1, PlanRegional AS PlanRegionalQ1, Cluster AS ClusterQ1
		FROM Proyecto P, TipoProyecto T, Estado E, Subsector SS, Sector S, Institucion I, Institucioncontacto JI, Contacto J, TipoIdentificacion TI, 
		Ciudad C, Departamento D, ProyectoMetaSocial M 
		WHERE 	P.codTipoProyecto=T.id_TipoProyecto AND P.codEstado=E.id_Estado AND 
		SS.id_subsector=P.codSubsector AND SS.codSector=S.id_sector AND P.id_Proyecto=M.codProyecto AND
		I.id_Institucion=P.codInstitucion AND I.id_institucion=JI.codInstitucion AND JI.codContacto=J.id_contacto AND JI.FechaFin is Null AND 
		J.codTipoIdentificacion=TI.id_TipoIdentificacion AND
		P.codCiudad=C.id_ciudad AND C.codDepartamento=D.id_departamento AND 
		id_Proyecto=@id_proyecto

	end

	-----------------------------------
	IF @caso=5

	begin
		SELECT P.Fecha AS FechaQ5, P.Aval AS AvalQ5, P.observaciones AS observacionesQ5, C.nombres AS nombresQ5, C.Apellidos AS ApellidosQ5
		FROM ProyectoFormalizacion P, Contacto C WHERE P.codContacto=C.id_contacto AND id_Proyectoformalizacion=@idproyectoformalizacion
	end
END