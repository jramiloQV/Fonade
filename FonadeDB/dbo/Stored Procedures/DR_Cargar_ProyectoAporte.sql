CREATE PROCEDURE [dbo].[DR_Cargar_ProyectoAporte] 
(
	@CodProyecto INT,
	@AporteID INT = NULL
)
AS
BEGIN
	SELECT 
		id_aporte,
		nombre, 
		valor, 
		tipoaporte, 
		detalle 
	FROM 
		proyectoaporte 
	WHERE 
		codproyecto = @CodProyecto AND
		id_aporte = ISNULL(@AporteID,id_aporte)
	ORDER BY 
		tipoaporte,
		nombre
END