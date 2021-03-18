CREATE PROCEDURE MD_Mostrar_resumen_equipo

	@RolEmprendedor int,
	@RolAsesor int,
	@RolAsesorLider int,
	@CodigoProyecto int,
	@caso varchar(10)

AS

BEGIN

	IF @caso='Equipo'
	
	begin
		select id_contacto, nombres + ' ' +apellidos as nombre, email, horasproyecto as horas, id_rol, r.nombre as rol
		from proyectocontacto pc, contacto, rol r
		where id_rol=codrol and id_contacto=codcontacto and  pc.inactivo=0
		and codrol in (@RolEmprendedor, @RolAsesor, @RolAsesorLider)
		and codproyecto =@CodigoProyecto
		order by nombres
	end

	IF @caso='Info'
	
	begin
		select nomproyecto, sumario, nomciudad, nomdepartamento, nomsubsector, nomunidad, nominstitucion, recursos
		from  ciudad, departamento, subsector, institucion, proyecto p LEFT OUTER JOIN proyectofinanzasingresos
		on id_proyecto=codproyecto where id_ciudad=p.codciudad and id_departamento=coddepartamento and id_subsector=codsubsector
		and id_institucion=codinstitucion and  id_proyecto=@CodigoProyecto
	end	

END