CREATE VIEW vw_vista_total3 AS
SELECT distinct b.Id_Proyecto,
b.Ciudad,
b.Empresa,
b.Id_Departamento,
b.Id_Sector,
b.nomciudad,
b.NomDepartamento,
b.recursos,
b.Sector,
CASE a.Empleos_Verificados WHEN 66 THEN 0 ELSE a.Empleos_Verificados END Empleos
FROM vw_vista_total2 b
LEFT JOIN A1320_Indicadores_X_Proyecto a
ON b.Id_Proyecto = a.Id