/*
	Fecha: 07/04/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_PersonalCalificadoSeleccionado_Nomina
	Descripción: Cargar los datos asociados a la nómina seleccionada de la grilla que usa el procedimiento
	almacenado "MD_ListaDePersonalCalificado_Nomina".
*/

CREATE PROCEDURE [dbo].[MD_PersonalCalificadoSeleccionado_Nomina]
(
	@CodProyecto INT,
	@CodNomina INT
)
AS
SELECT DISTINCT   *
FROM InterventorNomina AS a,InterventorNominaMes AS b
WHERE a.Tipo='Cargo' AND a.Id_Nomina = b.CodCargo AND a.CodProyecto = @CodProyecto--47876
AND b.Mes <> 0 and a.Id_Nomina = @CodNomina--26143
ORDER BY a.Id_Nomina, b.Mes, b.Tipo