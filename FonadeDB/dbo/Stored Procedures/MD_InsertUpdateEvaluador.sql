CREATE PROCEDURE [dbo].[MD_InsertUpdateEvaluador]

	  @nombres varchar(100)
	, @apellidos varchar(100)
	, @codtipoIdentificacion int
	, @identificacion float
	, @email varchar(100) 
	, @clave varchar(20)
	, @CodGrupo int
	, @Id_Contacto int
	, @persona varchar(1)
	, @caso varchar(10)
	
AS

BEGIN

	IF @caso='Create'

		begin

			Insert into contacto
			(nombres,apellidos,CodTipoIdentificacion,Identificacion,Email,Clave) 
			values (@nombres,@apellidos,@codtipoIdentificacion,@identificacion,@email,@clave)


			declare @CodContacto int
			select @CodContacto = Id_Contacto from Contacto where Email=@email

			DELETE FROM GrupoContacto WHERE CodContacto=@CodContacto

			INSERT INTO GrupoContacto(CodContacto,CodGrupo) 
			VALUES(@CodContacto,@CodGrupo)

			INSERT INTO Evaluador(CodContacto, Persona)
			VALUES (@CodContacto,@persona)

		end

	IF @caso='Update'

		begin

			Update Contacto set Nombres = @nombres
			,Apellidos = @apellidos
			,CodTipoIdentificacion = @codtipoIdentificacion
			,Identificacion = @identificacion
			,Email = @email
			WHERE Id_Contacto = @Id_Contacto

			UPDATE Evaluador 
			SET Persona =  @persona
			WHERE CodContacto = @Id_Contacto
		
		end

END