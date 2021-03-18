
CREATE PROCEDURE [dbo].[spObtenerCodNomina]
(
	@CodProyecto INT
)
AS
--SELECT top 1 * 
SELECT TOP 1 Id_Nomina
FROM InterventorNomina 
WHERE Tipo = 'Cargo' AND CodProyecto = 47876
ORDER BY id_Nomina