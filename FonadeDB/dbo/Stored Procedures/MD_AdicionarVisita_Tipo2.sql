/*
	Fecha: 19/05/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_AdicionarVisita_Tipo1
	Descripción: Permitir la inserción de visitas de tipo 2.
*/
CREATE PROCEDURE [dbo].[MD_AdicionarVisita_Tipo2]
(
	@FechaInicio SMALLDATETIME, 
	@FechaFin SMALLDATETIME,
	@Objeto VARCHAR(255),
	@Id_Visita INT
)
AS
UPDATE Visita SET FechaInicio = @FechaInicio, 
				  FechaFin = @FechaFin, 
				  Objeto = @Objeto
WHERE Id_Visita = @Id_Visita