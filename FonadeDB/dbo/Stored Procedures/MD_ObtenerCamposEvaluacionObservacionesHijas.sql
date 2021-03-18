/*
-- Author:		Alberto Palencia Benedetti
-- Create date: 2014-03-08
-- Description:	Obtenemos los campos dependiendo el id del aspecto
ejemplo: 

MD_ObtenerCamposEvaluacionObservacionesHijas 8,50161,203,1

Idcampo,codigoproyecto,codigoconvocatoria,codigoaspecto.
*/
CREATE PROCEDURE [dbo].[MD_ObtenerCamposEvaluacionObservacionesHijas]
	-- Add the parameters for the storred procedure here
	
	 @campo int -- este id representa el id del campo que posee la descripcion para asi poder cargar las observaciones que tiene 
	,@codProyecto int -- codigo del proyecto
	,@codConvocatoria int -- codigo de la convocatoria
	,@idCampo int -- id del campo para consultar las descripciones que tiene.
AS
BEGIN
	
	SET NOCOUNT ON;

		
WITH  Evaluacion(id_campo,campo,idVariable,Maximo,Asignado,orden)
	
AS (
		SELECT 
			C.id_campo,
		    c.campo
			, p.id_campo as idVariable
			,cc.Puntaje Maximo			
			,isnull(ec.Puntaje, 0) Asignado
			, case when cc.Puntaje is null then c.campo else p.campo end orden
		FROM CAMPO C 
		INNER JOIN CAMPO P ON c.codcampo=p.id_campo
		INNER JOIN ConvocatoriaCampo CC ON C.id_campo = CC.codCampo AND
		C.Inactivo = 0 AND ( p.codcampo=@idCampo OR c.codcampo=@idCampo) AND 
		CC.codConvocatoria= @codConvocatoria
		LEFT JOIN EvaluacionCampo EC ON C.id_campo = EC.codCampo AND
		CC.codConvocatoria=EC.codConvocatoria  AND 
		EC.codProyecto=@codProyecto
		)

	 SELECT 
		* 
	 FROM Evaluacion
	 WHERE idVariable = @campo
	 ORDER BY id_campo,Maximo 



END