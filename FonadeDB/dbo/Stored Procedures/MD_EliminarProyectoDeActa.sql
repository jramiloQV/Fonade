-- =============================================
-- Author:		Alberto Palencia Benedetti
-- Create date: 18/03/2014
-- Description:	Elimina el acta del proyecto
-- =============================================
CREATE PROCEDURE MD_EliminarProyectoDeActa
	-- Add the parameters for the stored procedure here
	@codProyecto int,
	@codConvocatoria int
	
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM AcreditacionActaProyecto 
	WHERE CODPROYECTO= @codProyecto  AND CODACTA in (SELECT ID_ACTA FROM AcreditacionActa WHERE CODCONVOCATORIA= @codConvocatoria)
END