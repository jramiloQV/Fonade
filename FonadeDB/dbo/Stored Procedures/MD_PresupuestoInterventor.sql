-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_PresupuestoInterventor] 
	-- Add the parameters for the stored procedure here
	@codProyecto INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CONST_EstadoInterventor int = 1

    -- Insert statements for procedure here
			SELECT
				PagoActividad.Id_PagoActividad, PagoActividad.NomPagoActividad, PagoActividad.FechaInterventor, PagoActividad.CantidadDinero,
			PagoActividad.Estado, PagoActividad.FechaRtaFA, PagoActividad.CodigoPago, PagoBeneficiario.Nombre,
			PagoBeneficiario.Apellido, PagoBeneficiario.RazonSocial, TipoIdentificacion.NomTipoIdentificacion, PagoBeneficiario.NumIdentificacion,
			PagoActividad.ValorReteFuente, PagoActividad.ValorReteIVA, PagoActividad.ValorReteICA, Pagoactividad.OtrosDescuentos, PagoActividad.ValorPagado, PagoActividad.ObservacionesFA
			FROM PagoActividad
			INNER JOIN PagoBeneficiario ON PagoActividad.CodPagoBeneficiario = PagoBeneficiario.Id_PagoBeneficiario
			INNER JOIN TipoIdentificacion ON PagoBeneficiario.CodTipoIdentificacion = TipoIdentificacion.Id_TipoIdentificacion
			WHERE (PagoActividad.Estado >= @CONST_EstadoInterventor)
			AND (dbo.PagoActividad.CodProyecto = @codProyecto)
			ORDER BY PagoActividad.FechaInterventor, 1
END