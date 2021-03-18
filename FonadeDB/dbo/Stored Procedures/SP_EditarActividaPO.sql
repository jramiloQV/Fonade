
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE SP_EditarActividaPO 
	-- Add the parameters for the stored procedure here
	@codActividad int,
	@mes int,
	@codTipoFinancia int,
	@observacion varchar(5120),
	@valor money

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE avanceactividadPOMes SET Valor = @valor, Observaciones = @observacion WHERE codActividad = @codActividad AND Mes= @mes AND CodTipoFinanciacion = @codTipoFinancia
END