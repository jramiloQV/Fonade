
/* =============================================
Author:		Wilson Guasca
alter date: 2010-06-23 12:50
Description: Consulta única de totalización de usuarios
  ACTIVOS agrupados por perfil (grupo)

EXECUTE [ConsultaContactosActivosPorGrupo]

Ajuste de de Consulta
============================================= */
CREATE PROCEDURE [dbo].[PTAL_Sector_Indicadores_Prueba]
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
					count(v.Sector) as Pctg
			FROM	(select * from vw_pruebas) v
			WHERE	 v.CodDepartamento = @iddepto
			GROUP BY v.NomDepartamento,v.sector
			ORDER BY v.NomDepartamento desc,count( v.Id_Proyecto) desc

			union all

			SELECT	'Otros',(SELECT	count(v.Id_Proyecto) as Total
					FROM	(select * from vw_pruebas) v
					WHERE	 v.CodDepartamento = @iddepto
					) - sum(a.Pctg) from (	SELECT top 5	v.sector as NomSector, 
											count(v.Sector) as Pctg
											FROM	(select * from vw_pruebas) v
											WHERE	 v.CodDepartamento = @iddepto
											GROUP BY v.NomDepartamento,v.sector
											ORDER BY v.NomDepartamento desc,count( v.Id_Proyecto) desc
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
		FROM	(select * from vw_pruebas) v
		GROUP BY v.sector
		ORDER BY Pctg desc 

		union all

		SELECT	'Otros',(SELECT	count(v.Id_Proyecto) as Total
				FROM	(select * from vw_pruebas) v
				) - sum(a.Pctg) from (	SELECT top 5	v.sector as NomSector, 
										count(v.Sector) as Pctg
										FROM	(select * from vw_pruebas) v
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
			FROM	vw_pruebas
			WHERE	CodDepartamento = @iddepto 
			--GROUP BY vw_pruebas.NomDepartamento
		)as Empresa,
		(
			SELECT COUNT(Distinct Ciudad) Municipios 
			FROM	vw_pruebas
			WHERE	CodDepartamento = @iddepto
			--GROUP BY vw_pruebas.NomDepartamento
		)as Municipios,
		(
		SELECT format(SUM (recursos),'C','es-CO')
			FROM (
					SELECT	e.ValorRecomendado * (SELECT SalarioMinimo FROM dbo.SalariosMinimos WHERE AñoSalario = (select (DatePart(yyyy,c.fechaInicio)))) AS recursos
					FROM	Evaluacionobservacion AS e
							INNER JOIN dbo.Convocatoria AS c ON c.Id_Convocatoria = e.CodConvocatoria
							INNER JOIN dbo.vw_pruebas AS pru ON pru.Id_Proyecto = e.CodProyecto
					WHERE	CodDepartamento = @iddepto
				) AS r
		)AS resursos,
		(
			SELECT SUM(Cargos) 
			FROM vw_pruebas
			WHERE		CodDepartamento = @iddepto
		)AS Empleos, 
		(	SELECT	top 1	NomDepartamento 
			FROM			vw_pruebas
			WHERE			CodDepartamento=@iddepto) as Departamento

	END
	
	IF (@municipios =4)
	BEGIN
	/*******CONSOLIDADO COLOMBIA***********************************************************************************************/
	select	(
				SELECT Distinct COUNT(*) Empresas FROM vw_pruebas 
			)as Empresa,
			(
				SELECT COUNT(Distinct Ciudad) Municipios FROM vw_pruebas
			)as Municipios,
			(
			SELECT format(SUM (recursos),'C','es-CO')
			FROM (
					SELECT e.ValorRecomendado * (SELECT SalarioMinimo FROM dbo.SalariosMinimos WHERE AñoSalario = (select (DatePart(yyyy,c.fechaInicio)))) AS recursos
					FROM Evaluacionobservacion AS e
					INNER JOIN dbo.Convocatoria AS c ON c.Id_Convocatoria = e.CodConvocatoria
					INNER JOIN dbo.vw_pruebas AS pru ON pru.Id_Proyecto = e.CodProyecto
					
				) AS r
			)AS recursos,
			(
				SELECT SUM(Cargos) FROM vw_pruebas
			)AS Empleo,'Colombia' as Departamento																		
	 END
END