-- =============================================
-- Author:		<Author,Alberto Palencia Benedetti>
-- Create date: <Create Date, 03-03-2014,>
-- Description:	<Description, Obtengo si el proyecto esta evaluado o no>
-- MD_obtenerTabs 50161
-- =============================================
CREATE PROCEDURE MD_obtenerTabs
	-- Add the parameters for the stored procedure here
	@idproyecto int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
declare @count  int = (SELECT isnull(COUNT(0),0) - 2 FROM TabEvaluacion WHERE codTabEvaluacion is NULL);
declare @proyect int  = 0;

	set @proyect =	(SELECT 
			codproyecto codproyecto
		FROM tabevaluacionproyecto tep, tabevaluacion te
		WHERE id_tabevaluacion=tep.codtabevaluacion  and realizado=1 
		and te.codtabevaluacion is null and codproyecto= @idproyecto
		and codconvocatoria in (
		 SELECT TOP 1 codConvocatoria
		 FROM ConvocatoriaProyecto CP 
		WHERE codProyecto = @idproyecto ORDER BY fecha DESC) 
		GROUP BY codproyecto
		HAVING count(tep.codtabevaluacion)= @count)

	Select ISNULL(@proyect,0)  codproyecto
END