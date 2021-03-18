

/* =============================================
Author:		Wilson Guasca
Modificado: Christiam Alejandro Niño
alter date: 2010-06-23 12:50
alter date: 2015-11-09 20:38
Description: Consulta de mapa

EXECUTE [ConsultaContactosActivosPorGrupo]
============================================= */
CREATE PROCEDURE [dbo].[PTAL_Sector_Can]
	@municipios int,
	@iddepto int
AS
BEGIN
 
	SET NOCOUNT ON;
	IF (@municipios =1)
		BEGIN
		
		select T.NomSector, T.Pctg FROM
		(select top 5 s.NomSector ,  count( p.Id_Proyecto) 
                    as Pctg 
                    	 
                     from departamento d
                    join ciudad c ON c.CodDepartamento = d.Id_Departamento 
                    join Proyecto p ON p.CodCiudad= c.Id_Ciudad
                    join  SubSector ss on ss.Id_SubSector = p.CodSubSector
                    join Sector s on s.Id_Sector =ss.CodSector
                    where p.CodEstado =7  and d.Id_Departamento = @iddepto
                    group by  d.NomDepartamento, s.NomSector
                    order by d.NomDepartamento desc,count( p.Id_Proyecto) desc 
					 UNION ALL
					 select 'Otros', (select    count( p.Id_Proyecto) 
                     as Pctg                     	 
                     from departamento d
                    join ciudad c ON c.CodDepartamento = d.Id_Departamento 
                    join Proyecto p ON p.CodCiudad= c.Id_Ciudad                    
                    where p.CodEstado =7  and d.Id_Departamento = @iddepto)  - count(A.Pctg)  from (
					 select  top 5  count( p.Id_Proyecto) 
                     as Pctg 
                    	 ,s.NomSector 
                     from departamento d
                    join ciudad c ON c.CodDepartamento = d.Id_Departamento 
                    join Proyecto p ON p.CodCiudad= c.Id_Ciudad
                    join  SubSector ss on ss.Id_SubSector = p.CodSubSector
                    join Sector s on s.Id_Sector =ss.CodSector
                    where p.CodEstado =7  and d.Id_Departamento = @iddepto
                    group by  d.NomDepartamento, s.NomSector
                    order by d.NomDepartamento desc,count( p.Id_Proyecto) desc 
					) A				
					) T

		END
	IF (@municipios =2)
		BEGIN
		SELECT t.Sector NomSector, t.Pctg FROM (
			SELECT top 5 v.sector, count(v.id_proyecto) as Pctg
			FROM vw_vista_total v
			GROUP BY v.sector
			ORDER BY count(v.id_proyecto) DESC 
			UNION ALL
			SELECT 'Otros',
				(SELECT	count(v.Id_Proyecto) as Total
				FROM
					(SELECT * FROM vw_vista_total) v) - sum(a.Pctg) 
					FROM 
						(SELECT top 5	v.sector as NomSector, 
						count(v.Sector) as Pctg
						FROM(SELECT * from vw_vista_total) v
				GROUP BY v.sector
				ORDER BY Pctg desc) a 
			)t
		END
	IF (@municipios =3)
		BEGIN
			SELECT 
				COUNT(DISTINCT v.id_proyecto) Empresa,
				COUNT(DISTINCT v.Ciudad) Municipios,
				FORMAT(SUM (v.recursos),'C','es-CO') resursos,
				SUM(v.Empleos) Empleos,
				(select d.NomDepartamento 
				from departamento d 
				where d.Id_Departamento=@iddepto) as Departamento
			FROM vw_vista_total v
			WHERE v.id_Departamento=@iddepto
			GROUP BY v.id_departamento
		END
	IF (@municipios =4)
		BEGIN
			SELECT 
			COUNT(DISTINCT v.id_proyecto) Empresa,
			COUNT(DISTINCT v.Ciudad) Municipios,
			FORMAT(SUM (v.recursos),'C','es-CO') recursos,
			SUM(v.Empleos) Empleo,
			'Colombia' as Departamento
			FROM vw_vista_total v
		END
	END