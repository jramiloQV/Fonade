
/*
	Fecha: 08/04/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_ListaDeCargosEnAprobacion_Nomina
	Descripción: Cargar los datos de las tablas mencionadas para generar la grilla en la página de
	Interventor - Nómina ("Mano de Obra al Plan Operativo").
*/

CREATE PROCEDURE [dbo].[MD_ListaDeCargosEnAprobacion_Nomina]
(
	@CodProyecto INT
)
AS
-- mofhb = Tabla de la parte inferior de la grilla de NOMINA!!
SELECT *
FROM dbo.InterventorNominaTMP
WHERE CodProyecto = @CodProyecto --47876
ORDER BY Tipo