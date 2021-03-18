-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [MD_DetalleSolicitudesPago]
	-- Add the parameters for the stored procedure here
	@codActa int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		SELECT PagoActividad.Id_PagoActividad, PagoActividad.FechaCoordinador, Empresa.razonsocial, PagoActividad.CantidadDinero, PagosActaSolicitudPagos.Aprobado
		FROM PagosActaSolicitudPagos
		INNER JOIN PagoActividad ON PagosActaSolicitudPagos.CodPagoActividad = PagoActividad.Id_PagoActividad
		INNER JOIN Empresa ON PagoActividad.CodProyecto = Empresa.codproyecto
		WHERE (PagosActaSolicitudPagos.CodPagosActaSolicitudes = @codActa)
END