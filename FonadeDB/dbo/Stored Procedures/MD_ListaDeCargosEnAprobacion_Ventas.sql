
/*
	Fecha: 16/04/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_ListaDeCargosEnAprobacion_Ventas
	Descripción: Cargar los datos de las tablas mencionadas para generar la grilla en la página de
	Interventor - Ventas ("Ventas en Aprobación").
*/

CREATE PROCEDURE [dbo].[MD_ListaDeCargosEnAprobacion_Ventas]
(
	@CodProyecto INT
)
AS
-- Tabla de la parte inferior de la grilla de Ventas!!
SELECT *
FROM InterventorVentasTMP 
WHERE codproyecto = @CodProyecto --47876 
ORDER BY id_ventas