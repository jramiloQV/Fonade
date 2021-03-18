/*
	Fecha: 11/04/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_ImprimirDetalles_Nomina
	Descripción: Cargar todos los detalles de los productos que tiene el proyecto, esto se aplica
	a la tabla que contendrá la información de "Nómina".
*/
CREATE PROCEDURE [dbo].[MD_ImprimirDetalles_Nomina]
(
	@CodProyecto INT
)
AS
SELECT DISTINCT *
FROM InterventorNomina AS a,InterventorNominaMes AS b
WHERE a.Tipo='Cargo' AND a.Id_Nomina = b.CodCargo 
AND a.CodProyecto = @CodProyecto --47876
AND b.Mes <> 0
ORDER BY a.Id_Nomina, b.Mes, b.Tipo