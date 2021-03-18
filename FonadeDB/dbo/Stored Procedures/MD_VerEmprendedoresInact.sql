
CREATE PROCEDURE [dbo].[MD_VerEmprendedoresInact]
	@CodInstitucion int,
	@CONST_RolEmprendedor int,
	@caso varchar(100),
	@igualwhere varchar(100)
AS

BEGIN

	IF @caso = 'Todo'
	Begin
		select distinct id_contacto, Nombres,apellidos,email,identificacion, Nominstitucion from contacto C, proyectocontacto PC, Institucion I 
		where CodInstitucion=Id_Institucion and (PC.inactivo=0 and C.inactivo=1)
		and CodContacto=id_contacto 
		and C.CodInstitucion=@CodInstitucion
		and codrol = @CONST_RolEmprendedor
		order by Nombres
	end
	ELSE
		IF @caso = 'Nombre'
		Begin
			select distinct id_contacto, Nombres,apellidos,email,identificacion, Nominstitucion from contacto C, proyectocontacto PC, Institucion I 
			where CodInstitucion=Id_Institucion and (PC.inactivo=0 and C.inactivo=1)
			and CodContacto=id_contacto 
			and C.CodInstitucion=@CodInstitucion
			and codrol = @CONST_RolEmprendedor
			and Nombres like '%' + @igualwhere + '%'
			order by Nombres
		end
		else
			IF @caso = 'Apellido'
			Begin
				select distinct id_contacto, Nombres,apellidos,email,identificacion, Nominstitucion from contacto C, proyectocontacto PC, Institucion I 
				where CodInstitucion=Id_Institucion and (PC.inactivo=0 and C.inactivo=1)
				and CodContacto=id_contacto 
				and C.CodInstitucion=@CodInstitucion
				and codrol = @CONST_RolEmprendedor
				and apellidos like '%' + @igualwhere + '%'
				order by Nombres
			end
			else
				IF @caso = 'email'
				Begin
					select distinct id_contacto, Nombres,apellidos,email,identificacion, Nominstitucion from contacto C, proyectocontacto PC, Institucion I 
					where CodInstitucion=Id_Institucion and (PC.inactivo=0 and C.inactivo=1)
					and CodContacto=id_contacto 
					and C.CodInstitucion=@CodInstitucion
					and codrol = @CONST_RolEmprendedor
					and email like '%' + @igualwhere + '%'
					order by Nombres
				end
				else
					IF @caso = 'Numero'
					Begin
						select distinct id_contacto, Nombres,apellidos,email,identificacion, Nominstitucion from contacto C, proyectocontacto PC, Institucion I 
						where CodInstitucion=Id_Institucion and (PC.inactivo=0 and C.inactivo=1)
						and CodContacto=id_contacto 
						and C.CodInstitucion=@CodInstitucion
						and codrol = @CONST_RolEmprendedor
						and convert(decimal(20,0), identificacion) like '%' + @igualwhere + '%'
						order by Nombres
					end
END