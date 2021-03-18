/*
	Desarrollador: Mauricio Arias Olave.
	Fecha: 16:44 20/06/2014
	Descripción: Ingresar registros (interventores) a la tabla "EmpresaInterventor".
*/

CREATE PROCEDURE MD_Insertar_New_EmpresaInterventor
(
	@CodEmpresa INT,
	@CodContacto INT,
	@Rol INT,
	@FechaInicio DATETIME
)
AS
INSERT INTO EmpresaInterventor (codempresa,codcontacto,rol,Fechainicio)
VALUES (@CodEmpresa, @CodContacto,@Rol,@FechaInicio)