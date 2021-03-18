/*
-- Author:		Alberto Palencia Benedetti
-- Create date: 2014-03-08
-- Description:	Obtenemos los campos dependiendo el id del aspecto
ejemplo: 

MD_ObtenerCamposEvaluacionObservaciones 50161,203,1

*/
CREATE PROCEDURE [dbo].[MD_ObtenerCamposEvaluacionObservaciones]
	-- Add the parameters for the storred procedure here
	
	@codProyecto int
	,@codConvocatoria int
	,@idCampo int 
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
				case when cc.Puntaje is null then c.campo else p.campo end orden
				,p.id_Campo CampoId
	FROM Campo C 
	LEFT JOIN Campo P ON c.codcampo=p.id_campo 
	INNER JOIN ConvocatoriaCampo CC ON C.id_campo = CC.codCampo AND 
	C.Inactivo = 0 AND (p.codcampo=@idCampo  OR c.codcampo= @idCampo) AND 
	CC.codConvocatoria = @codConvocatoria
	LEFT JOIN EvaluacionCampo EC ON C.id_campo = EC.codCampo AND
	CC.codConvocatoria = EC.codConvocatoria  AND  EC.codProyecto= @codProyecto
	group by case when cc.Puntaje is null then c.campo else p.campo end,p.id_Campo
	ORDER BY CampoId,orden
END