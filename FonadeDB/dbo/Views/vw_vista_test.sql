CREATE VIEW vw_vista_test AS
SELECT DISTINCT 
p.Id_Proyecto, 
em.Nit Empresa,
ci.Id_Ciudad Ciudad, 
d.Id_Departamento,
sec.Id_Sector,
sec.NomSector Sector,
d.NomDepartamento,
eo.ValorRecomendado * sm.SalarioMinimo as recursos,
c.Id_Convocatoria,
c.FechaInicio,
sm.SalarioMinimo,
eo.ValorRecomendado
FROM 
dbo.Proyecto p,
dbo.Estado e,
dbo.ConvocatoriaProyecto cp,
dbo.Convocatoria c,
dbo.Empresa em,
dbo.Ciudad ci,
dbo.Departamento d,
dbo.subsector ss,
dbo.Evaluacionobservacion eo,
dbo.sector sec,
dbo.proyectoaporte pa,
dbo.proyectogastospersonal pgp,
dbo.EmpleosGenerados eg,
dbo.SalariosMinimos sm,
(	select p.id_proyecto proyecto, max(cp.Fecha) fecha
	FROM dbo.ConvocatoriaProyecto cp, dbo.proyecto p
	WHERE cp.CodProyecto = p.Id_Proyecto	
	group by p.id_proyecto) as f
WHERE 
p.CodEstado = e.Id_Estado
AND sm.AñoSalario = (select (DatePart(yyyy,c.fechaInicio)))
AND p.Id_Proyecto = cp.CodProyecto
AND p.Id_Proyecto = eo.CodProyecto
AND p.Id_Proyecto = eg.CodProyecto
AND p.CodSubSector = ss.Id_SubSector
AND sec.Id_Sector = ss.CodSector
AND cp.CodConvocatoria = c.Id_Convocatoria
AND f.proyecto = p.Id_Proyecto
AND f.fecha = cp.fecha
AND cp.Viable = 1
AND em.codproyecto = p.Id_Proyecto
AND p.CodCiudad = ci.Id_Ciudad
AND ci.CodDepartamento = d.Id_Departamento
AND p.codEstado not in (4,11,10) 
AND p.Id_Proyecto = pgp.CodProyecto
AND c.Id_Convocatoria = eo.CodConvocatoria
AND p.Id_Proyecto NOT IN (
10812
,11749
,13875
,14620
,15717
,15904
,16282
,16568
,16720
,16805
,17550
,18135
,19565
,20516
,20581
,20771
,20803
,21210
,21284
,22168
,22248
,22295
,22420
,22422
,23306
,23363
,23465
,23466
,23582
,23710
,23919
,23923
,24102
,24581
,24646
,24753
,24978
,25146
,25240
,25241
,25546
,25560
,25711
,25773
,26261
,26333
,26638
,26640
,26740
,26744
,27121
,27171
,27204
,27239
,27357
,27396
,27399
,27427
,27430
,27464
,27507
,27643
,27648
,27656
,27664
,27684
,27685
,27753
,27759
,27801
,27807
,27853
,28085
,28092
,28116
,28119
,28159
,28201
,28209
,28240
,28370
,28417
,28432
,28474
,28501
,28513
,28515
,28522
,28557
,28572
,28746
,28867
,28892
,29130
,29168
,29195
,29230
,29247
,29248
,29417
,29455
,29476
,29477
,29579
,29583
,29587
,29711
,29864
,29882
,29926
,29934
,29956
,30006
,30046
,30211
,30221
,30300
,30317
,30354
,30976
,33134
,33275
,33331
,33332
,33512
,33531
,33589
,33976
,34111
,34152
,34425
,34764
,35064
,35489
,35579
,35793
,35920
,36066
,36150
,51458);