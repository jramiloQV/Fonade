-- =============================================
-- Author:		Alberto Palencia Benedetti
-- Create date: 16/03/2014
-- Description:	Obtiene todos los proyectos evaluados. con sus respectivos  puntajes para el detallado.

-- MD_ReporteDetalladoEvaluacion 10,''
-- =============================================
CREATE PROCEDURE MD_ReporteDetalladoEvaluacion
	@codconvocatoria int
	,@txtViable VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

IF @txtViable = '1'
	BEGIN
	   SET @txtViable = 'viable= 1' ;
	END 
ELSE IF @txtViable = '0'
	BEGIN
		  SET @txtViable = 'viable=0 and';
	END

ELSE
	BEGIN
		SET @txtViable = '';
	END

SELECT 
   id_proyecto
   ,nomproyecto
   ,nomciudad + '(' + d.NomDepartamento + ')' As nomciudad
   ,nomunidad + '(' + nominstitucion + ')'  As nominstitucion
   ,isnull(recursos,0) as montosolicitado
   ,isnull(valorrecomendado,0) as montorecomendado
   ,case when isnull(viable, 0)=1 then 'SI' else 'NO' end as viable
FROM proyecto  inner join convocatoriaproyecto cp on id_proyecto=cp.codproyecto AND  codconvocatoria= @codconvocatoria
		inner join ciudad on id_ciudad=codciudad 
		inner join departamento d on d.id_departamento=coddepartamento
		inner join subsector on id_subsector = codsubsector
		inner join sector on id_sector = codsector
		inner join institucion on id_institucion = codinstitucion
		left join proyectofinanzasingresos pfi on id_proyecto=pfi.codproyecto
		left join evaluacionobservacion ev on id_proyecto=ev.codproyecto
		and ev.codconvocatoria = cp.codconvocatoria
ORDER BY d.nomdepartamento
END