CREATE VIEW VW_PUNTAJES( Campo, Convocatoria, Puntaje, Codigo, Proyecto, Variable, Aspecto ) 
AS 
select c.campo, ec.codConvocatoria, ec.puntaje, ec.codproyecto, p.nomproyecto, v.campo, a.campo
from evaluacioncampo ec
inner join campo c on c.id_campo = ec.codcampo
inner join campo v on v.id_campo = c.codcampo 
inner join campo a on a.id_campo = v.codcampo 
inner join proyecto p on ec.codproyecto = p.id_proyecto