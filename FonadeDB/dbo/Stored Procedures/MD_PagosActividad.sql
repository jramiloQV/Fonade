-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [MD_PagosActividad]
	-- Add the parameters for the stored procedure here
	@CodUsuario int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		SELECT PagosActaSolicitudes.Id_Acta, PagosActaSolicitudes.Fecha, CONVERT(varchar(8000), .PagosActaSolicitudes.DatosFirma) AS DatosFirma, PagosActaSolicitudes.CodRechazoFirmaDigital
		FROM PagosActaSolicitudes
		INNER JOIN PagosActaSolicitudPagos ON PagosActaSolicitudes.Id_Acta = PagosActaSolicitudPagos.CodPagosActaSolicitudes
		WHERE (PagosActaSolicitudes.Tipo = 'Fonade')
		GROUP BY PagosActaSolicitudes.Id_Acta, PagosActaSolicitudes.Fecha, CONVERT(varchar(8000), .PagosActaSolicitudes.DatosFirma), PagosActaSolicitudes.CodRechazoFirmaDigital, PagosActaSolicitudes.CodContacto
		HAVING (PagosActaSolicitudes.CodContacto = @CodUsuario)
		ORDER BY FECHA DESC
END