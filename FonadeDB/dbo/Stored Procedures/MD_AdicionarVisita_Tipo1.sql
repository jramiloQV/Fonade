/*
	Fecha: 19/05/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_AdicionarVisita_Tipo1
	Descripción: Permitir la inserción de visitas de tipo 1.
*/
CREATE PROCEDURE [dbo].[MD_AdicionarVisita_Tipo1]
(
	@Id_Interventor INT, 
	@Id_Empresa INT, 
	@FechaInicio SMALLDATETIME, 
	@FechaFin SMALLDATETIME, 
	@Estado CHAR(24), 
	@Objeto VARCHAR(255)
)
AS
INSERT INTO Visita(Id_Interventor, Id_Empresa, FechaInicio, FechaFin, Estado, Objeto)
VALUES(@Id_Interventor, @Id_Empresa, @FechaInicio, @FechaFin, @Estado, @Objeto)