/*
	Desarrollador: Mauricio Arias Olave.
	Fecha: 16:12 20/06/2014
	Descripción: Actualizar los registros de la tabla "EmpresaInterventor" para 
	asignar interventores de la empresa seleccionada.
*/

CREATE PROCEDURE MD_Update_EmpresaInterventor
(
	@Fechafin DATETIME,
	@CodEmpresa INT,
	@CONST_RolInterventor INT,
	@CONST_RolInterventorLider INT
)
AS
UPDATE EmpresaInterventor SET Fechafin = @Fechafin, Inactivo = 1
WHERE CodEmpresa = @CodEmpresa
AND Rol IN (@CONST_RolInterventor, @CONST_RolInterventorLider)