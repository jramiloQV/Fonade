/*

	Nombre: MD_Create_InformeBimensual.
	Fecha: 13:08 16/07/2014
	Descripción: Procedimiento almacenado para guardar el nuevo informe bimensual; 
	éste procedimiento se invoca desde la ventana "AdicionarInformeBimensual".

*/
CREATE PROCEDURE MD_Create_InformeBimensual
(
	@NombreEmpresa VARCHAR(255),
	@codinterventor INT,
	@codempresa INT,
	@Periodo INT,
	@Fecha DATETIME
)
AS
INSERT INTO InformeBimensual (NomInformeBimensual, codinterventor, codempresa, Estado, Periodo, Fecha)
VALUES (@NombreEmpresa, @codinterventor, @codempresa, 0, @Periodo, @Fecha)