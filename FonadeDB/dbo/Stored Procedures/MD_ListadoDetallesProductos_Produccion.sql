
/*
	Fecha: 08/04/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_ListadoDetallesProductos_Produccion
	Descripción: Cargar los datos de las tablas mencionadas para generar la grilla en la página de
	Interventor - Producción ("Detalles al seleccionar el producto (grilla izquierda de la página.)").
*/

CREATE PROCEDURE [dbo].[MD_ListadoDetallesProductos_Produccion]
(
	@CodProyecto INT,
	@CodProduccion INT
)
AS 
SELECT DISTINCT * 
FROM dbo.InterventorProduccion LEFT JOIN dbo.InterventorProduccionMes
on id_produccion = CodProducto 
WHERE CodProyecto = @CodProyecto --47876
AND id_produccion = @CodProduccion --15689 --"CodProduccion"
ORDER BY id_produccion, mes, tipo