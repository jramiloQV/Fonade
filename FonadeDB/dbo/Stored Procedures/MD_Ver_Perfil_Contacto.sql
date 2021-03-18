
CREATE PROCEDURE [dbo].[MD_Ver_Perfil_Contacto]

	@IdContacto int,
	@caso varchar(10)

AS

BEGIN

	IF @caso='Asesores'
	
	begin
		SELECT 
		CASE WHEN CAST( dedicacion as varchar(20) )  = '0' THEN 'Completa'
		ELSE 'Parcial' END as dedicacion
		, Id_Contacto
		, nombres +' '+apellidos as nombre
		, email
		, ISNULL(experiencia, '') as experiencia
		, ISNULL(Hojavida, '') as Hojavida
		, ISNULL(intereses, '') as intereses
		FROM Contacto
		WHERE  Id_Contacto =@IdContacto
	end

	IF @caso='Otros'
	
	begin
		SELECT 
		CASE WHEN NomCiudad IS NULL THEN ''
		ELSE NomCiudad + ' (' + NomDepartamento + ')' END as NomCiudad1
		, Id_Contacto as Id_Contacto1
		, nombres +' '+apellidos as nombre1
		, email as email1
		, ISNULL(FechaNacimiento, '') as FechaNacimiento1
		, ISNULL(Telefono, '') as Telefono1
		FROM Contacto 
		Left join Ciudad on id_ciudad=codciudad
		left join Departamento on id_departamento=coddepartamento 
		WHERE  Id_Contacto =@IdContacto
	end	

END