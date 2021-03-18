

CREATE FUNCTION fn_PuntajeResultadoEvaluacion(@CodProyecto int,@CodConvocatoria int)
returns int
AS 
BEGIN
   DECLARE @PuntajeMinimo int,
   	   @Puntaje int,
	   @Puntos int
  
   select @PuntajeMinimo = sum(puntaje) from convocatoriacampo cc, campo c
   where id_campo=cc.codcampo and c.codcampo is null 
   and codconvocatoria=@CodConvocatoria
  
   select  @Puntaje = sum(ec.puntaje)  from evaluacioncampo ec 
   inner join campo c on c.id_campo = ec.codcampo 
   inner join campo v on v.id_campo = c.codcampo 
   inner join campo a on a.id_campo = v.codcampo 
   where codproyecto=@CodProyecto and CodConvocatoria=@CodConvocatoria

   if (@Puntaje>@PuntajeMinimo)
	set @Puntos = 10
   else
	set @Puntos = 0	
  
   return @Puntos
End