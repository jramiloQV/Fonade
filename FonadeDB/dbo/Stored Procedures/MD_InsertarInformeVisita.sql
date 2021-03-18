/*
	Fecha: 19/05/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_AdicionarVisita_Tipo1
	Descripción: Permitir la inserción de informes de vistas, se usa un procedimiento
	almacenado como solución al tratamiento de fechas DATETIME y SMALLDATETIME.
*/
CREATE PROCEDURE [dbo].[MD_InsertarInformeVisita]
(
	@NombreInforme VARCHAR(255), 
	@CodCiudadOrigen INT, 
	@CodCiudadDestino INT, 
	@FechaSalida SMALLDATETIME, 
	@FechaRegreso SMALLDATETIME, 
	@CodEmpresa INT, 
	@CodInterventor INT, 
	@InformacionTecnica TEXT, 
	@InformacionFinanciera TEXT, 
	@Estado BIT
)
AS
INSERT INTO InformeVisitaInterventoria(NombreInforme, CodCiudadOrigen, CodCiudadDestino, FechaSalida, 
									   FechaRegreso, CodEmpresa, FechaInforme, CodInterventor, 
									   InformacionTecnica, InformacionFinanciera, Estado) 
VALUES(@NombreInforme, @CodCiudadOrigen, @CodCiudadDestino, @FechaSalida, 
									   @FechaRegreso, @CodEmpresa, GETDATE(), @CodInterventor, 
									   @InformacionTecnica, @InformacionFinanciera, @Estado)