-- =============================================
-- Author:		Alberto Palencia Benedetti
-- Create date:  15/03/2014
-- Description:	 Obtener las funciones para ejecutarlas.
-- =============================================
CREATE PROCEDURE MD_ObtenerFuncionesProyectoNegocio
	-- Add the parameters for the stored procedure here
	@codconvocatoria int 
			
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select  id_criterio
		, nomcriterio
		, sigla
		, query + '('+cp.Parametros+')' as query
		,codconvocatoria
		, isnull(ccp.parametros,'') parametros
		, incidencia

from convocatoriacriteriopriorizacion ccp, criteriopriorizacion cp 

where id_criterio=codcriteriopriorizacion and codconvocatoria = @codconvocatoria

END