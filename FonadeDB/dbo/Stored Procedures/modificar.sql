create procedure modificar
@codproyecto		as	int,
@codconvoca			as	int,
@riesgo				as  varchar(500),
@mitigacion			as  varchar(500)
as

update EvaluacionRiesgo set Riesgo=@riesgo, Mitigacion=@mitigacion 
WHERE  codProyecto = @codproyecto and codConvocatoria=@codconvoca