
/*
	Fecha: 08/04/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_ProductosEnProduccion
	Descripción: Cargar los datos de las tablas mencionadas para generar la grilla en la página de
	Interventor - Producción.

	Actualización 1: "11/04/2014": Se cambia la consulta para generar botones de eliminación en
	desarrollo; se puede consultar los procedimientos "MD_ProductosEnProduccion" 
	y "MD_ListaDePersonalCalificado_Nomina" (entre otros...) para mayor aclaración y/o el código fuente
	de FONADE clásico.
*/

CREATE PROCEDURE [dbo].[MD_ProductosEnProduccion]
(
	@CodProyecto INT
)
AS
---- Versión 1.
--SELECT * 
--FROM dbo.InterventorProduccion
--WHERE CodProyecto = @CodProyecto
--ORDER BY id_produccion

---- Versión 2-
SELECT int_produccion.id_produccion, int_produccion.CodProyecto, int_produccion.NomProducto,
	   ISNULL((SELECT TOP 1 a_produccion.CodProducto
FROM dbo.AvanceProduccionPOMes AS a_produccion
WHERE a_produccion.CodProducto = int_produccion.id_produccion),0) AS ProduccionID
FROM dbo.InterventorProduccion AS int_produccion
WHERE codproyecto = @CodProyecto --47876