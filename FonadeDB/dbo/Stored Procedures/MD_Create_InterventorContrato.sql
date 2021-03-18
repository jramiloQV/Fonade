/*

	Nombre: MD_Create_InterventorContrato.
	Fecha: 10:58 14/07/2014
	Descripción: Procedimiento almacenado para guardar la información en la tabla "InterventorContrato"; 
	éste procedimiento se invoca desde la ventana "CatalogoContratoInterventor.aspx".

*/
CREATE PROCEDURE MD_Create_InterventorContrato
(
	@CodContacto INT,
	@NumContacto INT,
	@FechaInicio SMALLDATETIME,
	@FechaExpiracion SMALLDATETIME
)
AS
INSERT INTO InterventorContrato (CodContacto, NumContrato, FechaInicio, FechaExpiracion)
VALUES (@CodContacto, @NumContacto,@FechaInicio,@FechaExpiracion)