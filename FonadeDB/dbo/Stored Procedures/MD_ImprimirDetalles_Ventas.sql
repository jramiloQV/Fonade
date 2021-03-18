/*
	Fecha: 11/04/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_ImprimirDetalles_Ventas
	Descripción: Cargar todos los detalles de los productos que tiene el proyecto, esto se aplica
	a la tabla que contendrá la información de "Ventas".
*/
CREATE PROCEDURE [dbo].[MD_ImprimirDetalles_Ventas]
(
	@CodProyecto INT
)
AS
SELECT DISTINCT * 
FROM dbo.InterventorVentas LEFT JOIN dbo.InterventorVentasMes
ON id_ventas = CodProducto 
WHERE CodProyecto = @CodProyecto --47876
ORDER BY id_ventas, Mes, Tipo