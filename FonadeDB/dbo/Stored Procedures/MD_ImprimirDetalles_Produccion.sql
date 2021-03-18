/*
	Fecha: 11/04/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_ImprimirDetalles_Produccion
	Descripción: Cargar todos los detalles de los productos que tiene el proyecto, esto se aplica
	a la tabla que contendrá la información de "Producción".
*/
CREATE PROCEDURE [dbo].[MD_ImprimirDetalles_Produccion]
(
	@CodProyecto INT
)
AS
SELECT DISTINCT * 
FROM dbo.InterventorProduccion LEFT JOIN dbo.InterventorProduccionMes
on id_produccion = CodProducto 
WHERE CodProyecto = @CodProyecto --47876
ORDER BY id_produccion, mes, tipo