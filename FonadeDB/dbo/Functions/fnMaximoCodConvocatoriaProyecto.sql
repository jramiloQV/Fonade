create function [dbo].[fnMaximoCodConvocatoriaProyecto]
(
	@codProyecto int
)
returns varchar(MAX)
as
begin

	DECLARE  @codConvocatoriaMax int
	
	set @codConvocatoriaMax = (select max(codConvocatoria) as codConvocatoria 
	from dbo.ConvocatoriaProyecto where codProyecto = @codProyecto and viable = 1)
	

	return @codConvocatoriaMax
END