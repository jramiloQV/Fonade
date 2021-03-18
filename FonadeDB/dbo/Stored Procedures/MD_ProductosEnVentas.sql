
/*
	Fecha: 08/04/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_ProductosEnVentas
	Descripción: Cargar los datos de las tablas mencionadas para generar la grilla en la página de
	Interventor - Ventas.

	Actualización 1: "11/04/2014": Se cambia la consulta para generar botones de eliminación en
	desarrollo; se puede consultar los procedimientos "MD_ProductosEnProduccion" 
	y "MD_ListaDePersonalCalificado_Nomina" (entre otros...) para mayor aclaración y/o el código fuente
	de FONADE clásico.
*/

CREATE PROCEDURE [dbo].[MD_ProductosEnVentas]
(
	@CodProyecto INT
)
AS
---- Versión 1.
--SELECT * 
--FROM dbo.InterventorVentas
--WHERE CodProyecto = @CodProyecto
--ORDER BY id_ventas


-- Versión 2.
-- Ventas, grilla de la izquierda.
SELECT int_ventas.id_ventas, int_ventas.CodProyecto, int_ventas.NomProducto,
	   ISNULL((SELECT TOP 1 a_ventas.CodProducto
FROM dbo.AvanceVentasPOMes AS a_ventas
WHERE a_ventas.CodProducto = int_ventas.id_ventas),0) AS VentasID
FROM dbo.InterventorVentas AS int_ventas
WHERE codproyecto = @CodProyecto --47876