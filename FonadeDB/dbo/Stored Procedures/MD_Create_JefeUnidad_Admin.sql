/*

	Nombre: MD_Create_JefeUnidad_Admin.
	Fecha: 17:56 25/06/2014
	Descripción: Procedimiento almacenado para guardar el nuevo jefe de unidad; éste procedimiento
	se llama para crear un nuevo jefe de unidad desde la pantalla "SeleccionarJefeUnidad".

*/
CREATE PROCEDURE MD_Create_JefeUnidad_Admin
(
	@Nombres VARCHAR(100), 
	@Apellidos VARCHAR(100), 
	@CodTipoIdentificacion INT, 
	@Identificacion FLOAT, 
	@Email VARCHAR(100), 
	@Clave VARCHAR(20), 
	@CodCiudad INT, 
	@Telefono VARCHAR(100), 
	@Fax VARCHAR(100), 
	@Cargo VARCHAR(100)
)
AS
INSERT INTO Contacto (Nombres, Apellidos, CodTipoIdentificacion, Identificacion, Email, Clave, CodCiudad, Telefono, Fax, Cargo, fechaActualizacion)
VALUES(@Nombres, @Apellidos, @CodTipoIdentificacion, @Identificacion, @Email, @Clave, @CodCiudad, @Telefono, @Fax, @Cargo, GETDATE())