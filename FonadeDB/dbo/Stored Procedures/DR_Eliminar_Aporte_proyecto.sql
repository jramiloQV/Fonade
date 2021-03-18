CREATE PROCEDURE [dbo].[DR_Eliminar_Aporte_proyecto] 
(
	@AporteID INT
)
AS
BEGIN
	DELETE 
		proyectoaporte 
	WHERE 
		Id_Aporte = @AporteID  
END