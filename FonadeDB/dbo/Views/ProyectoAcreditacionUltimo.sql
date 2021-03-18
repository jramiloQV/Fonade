create view ProyectoAcreditacionUltimo as
Select pa.* from proyectoAcreditacion pa inner join
(
select Max(Id_ProyectoAcreditacion) as Id_ProyectoAcreditacion from Proyectoacreditacion 
Group by codProyecto, CodConvocatoria
)Tbl on pa.Id_ProyectoAcreditacion = tbl.Id_ProyectoAcreditacion