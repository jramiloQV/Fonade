
/*
	Fecha: 16/04/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_ListaDeCargosEnAprobacion_Produccion
	Descripción: Cargar los datos de las tablas mencionadas para generar la grilla en la página de
	Interventor - Producción ("Productos en Aprobación").
*/

CREATE PROCEDURE [dbo].[MD_ListaDeCargosEnAprobacion_Produccion]
(
	@CodProyecto INT
)
AS
-- Tabla de la parte inferior de la grilla de PRODUCCIÓN!!
SELECT *
FROM InterventorProduccionTMP 
WHERE codproyecto = @CodProyecto --47876 
ORDER BY Id_Produccion