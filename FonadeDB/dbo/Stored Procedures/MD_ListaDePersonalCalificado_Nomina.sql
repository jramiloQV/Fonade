/*
	Fecha: 05/04/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: spListaDePersonalCalificado_Nomina
	Descripción: Cargar los datos de las tablas mencionadas para generar la grilla en la página de
	Interventor - Nómina.

	Actualización 1 "07/04/2014": Se cambia la consulta, revisando bien el archivo "InterNominaIzq.asp".

	Actualización 2: "11/04/2014": Se corrige nuevamente la consulta, ya que no se tenía conocimiento
	de que LA TABLA "AvanceCargo" es a la que se refiere cuando se maneja información de Nómina.
*/

CREATE PROCEDURE [dbo].[MD_ListaDePersonalCalificado_Nomina]
(
	--@CodCargo INT,
	@CodProyecto INT
	--@CodNomina INT
)
AS
---- Versión 1
-- Mauricio Arias Olave. "": Se cambia por la consulta que Si es correcta, de acuerdo al código fuente.
--SELECT DISTINCT a.Id_Nomina, a.CodProyecto, a.Cargo, a.Tipo, b_.CodCargo, b_.Mes, b_.Valor, b_.Tipo
--FROM InterventorNomina a, InterventorNominaMes b_
--WHERE a.tipo='Cargo' AND a.Id_Nomina= @CodCargo AND a.CodProyecto= @CodProyecto
--AND b_.Mes <> 0 and a.Id_Nomina = @CodNomina
--ORDER BY a.Id_Nomina, b_.Mes, b_.Tipo
--Consulta real.

---- Versión 2
--SELECT * 
--FROM InterventorNomina 
--WHERE Tipo='Cargo'
--AND CodProyecto = @CodProyecto
--ORDER BY Id_Nomina

---- Versión 3
SELECT int_nomina.Id_Nomina, int_nomina.CodProyecto, int_nomina.Cargo, int_nomina.Tipo,
	   ISNULL((SELECT TOP 1 a_nomina.CodCargo
FROM AvanceCargoPOMes AS a_nomina
WHERE a_nomina.CodCargo = int_nomina.Id_Nomina),0) AS NominaID
FROM dbo.InterventorNomina AS int_nomina
WHERE int_nomina.CodProyecto = @CodProyecto --47876