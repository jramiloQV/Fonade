
/*
	Fecha: 08/04/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_ListadoDetallesProductos_Venta
	Descripción: Cargar los datos de las tablas mencionadas para generar la grilla en la página de
	Interventor - Ventas ("Detalles al seleccionar el producto (grilla izquierda de la página.)").
*/

CREATE PROCEDURE [dbo].[MD_ListadoDetallesProductos_Venta]
(
	@CodProyecto INT,
	@CodVentas INT
)
AS
SELECT DISTINCT * 
FROM dbo.InterventorVentas LEFT JOIN dbo.InterventorVentasMes
ON id_ventas = CodProducto 
WHERE CodProyecto = @CodProyecto --47876
AND id_ventas = @CodVentas --11841 --" & fnRequest("CodVentas") & _
ORDER BY id_ventas, Mes, Tipo