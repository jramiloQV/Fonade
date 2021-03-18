/* =============================================
Author:		Wilson Guasca
alter date: 2010-06-21 13:00
Description: Consulta el registro de la tabla
  [ServicioRegistroDesactivacion] para el cual 
  no se ha ejecutado el envio de reportes.
============================================= */
CREATE PROCEDURE [dbo].[ObtenerRegistroDesactivacionNoEnviado]
	--@CodRegistro Int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP (1) 
			Id_ServicioRegistroDesactivacion,  
			FechaReferencia,  
			TotalDesactivados,  
			FinDesactivacion,  
			InicioDesactivacion,  
			FechaProcesoNotificacion
	FROM ServicioRegistroDesactivacion
	WHERE FechaProcesoNotificacion IS NULL
	Order By InicioDesactivacion Desc
END