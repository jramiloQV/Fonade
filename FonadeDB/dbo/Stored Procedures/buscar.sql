create procedure buscar
@codproyecto		as	int,
@codconvoca			as	int
as
SELECT id_Riesgo, Riesgo, Mitigacion 
					FROM EvaluacionRiesgo 					
					WHERE codProyecto = @codproyecto and codConvocatoria=@codconvoca
					ORDER BY id_Riesgo