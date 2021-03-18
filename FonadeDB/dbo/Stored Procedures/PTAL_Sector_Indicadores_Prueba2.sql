
/* =============================================
Author:		Wilson Guasca
alter date: 2010-06-23 12:50
Description: Consulta única de totalización de usuarios
  ACTIVOS agrupados por perfil (grupo)

EXECUTE [ConsultaContactosActivosPorGrupo]

Ajuste de de Consulta
============================================= */
CREATE PROCEDURE [dbo].[PTAL_Sector_Indicadores_Prueba2]
	@municipios int,
	@iddepto int

AS
BEGIN
 
	SET NOCOUNT ON;
	/*******SECTORES DEPARTAMENTOS***************************************************************************************************/
	IF (@municipios =1)
	BEGIN

		SELECT	NomSector, 
				Pctg
		FROM
		(	SELECT	top 5	v.sector as NomSector, 
					--v.sector as NomSector, 
					count(v.Sector) as Pctg
			FROM	(select * from vw_f2) v
			WHERE	 v.Id_Departamento = @iddepto
			GROUP BY v.Departamento,v.sector
			ORDER BY v.Departamento desc,count( v.Id_Proyecto) desc

			union all

			SELECT	'Otros',(SELECT	count(v.Id_Proyecto) as Total
					FROM	(select * from vw_f2) v
					WHERE	 v.Id_Departamento = @iddepto
					) - sum(a.Pctg) from (	SELECT top 5	v.sector as NomSector, 
											count(v.Sector) as Pctg
											FROM	(select * from vw_f2) v
											WHERE	 v.Id_Departamento = @iddepto
											GROUP BY v.Departamento,v.sector
											ORDER BY v.Departamento desc,count( v.Id_Proyecto) desc
					) a 
		)t
		
		
	END
	/*******SECTORES COLOMBIA***************************************************************************************************/
	IF (@municipios =2)
	BEGIN
		
	SELECT	NomSector, 
			Pctg
	FROM
	(	SELECT	top 5 v.sector as NomSector, 
				count(v.Sector) as Pctg
		FROM	(select * from vw_f2) v
		GROUP BY v.sector
		ORDER BY Pctg desc 

		union all

		SELECT	'Otros',(SELECT	count(v.Id_Proyecto) as Total
				FROM	(select * from vw_f2) v
				) - sum(a.Pctg) from (	SELECT top 5	v.sector as NomSector, 
										count(v.Sector) as Pctg
										FROM	(select * from vw_f2) v
										GROUP BY v.sector
										ORDER BY Pctg desc 
				) a 
	)t
	END

	IF (@municipios =3)
	BEGIN
/*******CONSOLIDADO DEPARTAMENTOS*******************************************************************************************/
	SELECT (
			SELECT Distinct COUNT(Empresas) Empresas 
			FROM	vw_f2
			WHERE	Id_Departamento = @iddepto 
			--GROUP BY vw_pruebas.NomDepartamento
		)as Empresa,
		(
			SELECT COUNT(Distinct Ciudad) Municipios 
			FROM	vw_f2
			WHERE	Id_Departamento = @iddepto
			--GROUP BY vw_pruebas.NomDepartamento
		)as Municipios,
		(
		SELECT format(SUM (recursos),'C','es-CO')
			FROM (
					SELECT	e.ValorRecomendado * (SELECT SalarioMinimo FROM dbo.SalariosMinimos WHERE AñoSalario = (select (DatePart(yyyy,c.fechaInicio)))) AS recursos
					FROM	Evaluacionobservacion AS e
							INNER JOIN dbo.Convocatoria AS c ON c.Id_Convocatoria = e.CodConvocatoria
							INNER JOIN dbo.vw_f2 AS pru ON pru.Id_Proyecto = e.CodProyecto
					WHERE	Id_Departamento = @iddepto
				) AS r
		)AS resursos,
		(
			SELECT SUM([Id_Cargo]) 
			FROM vw_f1
			WHERE		Id_Departamento = @iddepto
		)AS Empleos, 
		(	SELECT	top 1	Departamento 
			FROM			vw_f2
			WHERE			Id_Departamento=@iddepto) as Departamento

	END
	
	IF (@municipios =4)
	BEGIN
	/*******CONSOLIDADO COLOMBIA***********************************************************************************************/
	select	(
				SELECT Distinct COUNT(*) Empresas FROM vw_f4 
			)as Empresa,
			(
				SELECT COUNT(Distinct Ciudad) Municipios FROM vw_f3
			)as Municipios,
			(
			SELECT format(SUM (recursos),'C','es-CO')
			FROM (
					SELECT e.ValorRecomendado * (SELECT SalarioMinimo FROM dbo.SalariosMinimos WHERE AñoSalario = (select (DatePart(yyyy,c.fechaInicio)))) AS recursos
					FROM Evaluacionobservacion AS e
					INNER JOIN dbo.Convocatoria AS c ON c.Id_Convocatoria = e.CodConvocatoria
					INNER JOIN dbo.vw_f2 AS pru ON pru.Id_Proyecto = e.CodProyecto
					
				) AS r
			)AS recursos,
			(
				SELECT Count([Id_Cargo]) FROM [vw_f1]
			)AS Empleo,'Colombia' as Departamento																		
	 END
END