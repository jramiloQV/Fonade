/*
	Fecha: 10/09/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_CrearEmprendedor
	Descripción: Registrar la información en la tabla "Contacto" acerca del emprendedor 
	(que está siendo creado en la opción "Crear Plan e Negocio - Adicionar Emprendedor).
*/
CREATE PROCEDURE [dbo].[MD_CrearEmprendedor_mod]
(
	@Nombres VARCHAR(100), 
	@Apellidos VARCHAR(100), 
	@CodTipoIdentificacion INT, 
	@Identificacion FLOAT, 
	@Email VARCHAR(100), 
	@Clave VARCHAR(20), 
	@CodInstitucion INT, 
	@CodTipoAprendiz INT, 
	@Genero CHAR(1),
	@FechaNacimiento SMALLDATETIME,
	@CodCiudad INT,
	@Telefono VARCHAR(100),
	@LugarExpedicionDI INT
)
AS
IF(SELECT COUNT(1) FROM Contacto c WHERE c.Email = @Email) = 0
BEGIN
	INSERT INTO Contacto (Nombres, Apellidos, CodTipoIdentificacion, Identificacion, Email, Clave, CodInstitucion, CodTipoAprendiz, Genero, FechaNacimiento, CodCiudad, Telefono, LugarExpedicionDI)
	VALUES(@Nombres, @Apellidos, @CodTipoIdentificacion, @Identificacion, @Email, @Clave, @CodInstitucion, @CodTipoAprendiz, @Genero, @FechaNacimiento, @CodCiudad, @Telefono, @LugarExpedicionDI)
	 SELECT 1
END
ELSE
BEGIN
	SELECT 0
End