CREATE PROCEDURE [dbo].[MD_VerFormalizacion]
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
	FROM ProyectoFormalizacion WHERE CodProyecto = @id_proyecto ORDER BY Id_ProyectoFormalizacion DESC
	
	-----------------------------------
	
	IF @caso=2

	begin
		SELECT R.nombre AS nombreQ2, C.nombres AS nombresQ2, C.apellidos AS apellidosQ2, id_rol AS id_rolQ2
		FROM ProyectoContacto P, Rol R, Contacto C 
		WHERE	R.id_rol=P.codRol AND C.id_Contacto=P.codContacto AND P.inactivo=0 AND codProyecto=@id_proyecto
		AND (codRol=@CONST_RolAsesor or Codrol=@CONST_RolAsesorLider)
		ORDER BY id_rol, nombres, apellidos
	end
	-----------------------------------
	IF @caso=3

	begin
		select id_cargo AS id_cargoQ3, cargo AS cargoQ3, valormensual AS valormensualQ3, 
		GeneradoPrimerAno AS GeneradoPrimerAnoQ3, Joven AS JovenQ3, Desplazado AS DesplazadoQ3,
		Madre AS MadreQ3, Minoria AS MinoriaQ3, Recluido AS RecluidoQ3, Desmovilizado AS DesmovilizadoQ3, 
		Discapacitado AS DiscapacitadoQ3, Desvinculado AS DesvinculadoQ3
		from proyectogastospersonal left outer join proyectoempleocargo
		on id_cargo=codcargo where codproyecto=@id_proyecto
	end
	
	-----------------------------------
	IF @caso=4

	begin
		SELECT C.Nombres as NombresQ4, C.Apellidos as ApellidosQ4, P.participacion AS participacionQ4, P.beneficiario AS beneficiarioQ4
		FROM ProyectoContacto P, contacto C 
		WHERE P.codcontacto=C.id_contacto AND P.codProyecto=@id_proyecto AND P.Inactivo=0 AND P.codRol=@CONST_RolEmprendedor
		ORDER BY C.Nombres, C.Apellidos
	end
	
END