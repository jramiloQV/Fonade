

/* =============================================
Author:		Wilson Guasca
alter date: 2010-06-23 12:50
Description: Consulta única de totalización de usuarios
  ACTIVOS agrupados por perfil (grupo)

EXECUTE [ConsultaContactosActivosPorGrupo]
============================================= */
CREATE PROCEDURE [dbo].[PTAL_Sector_Indicadores_backup]
	--@FechaReferencia DateTime, 
	@municipios int,
	@iddepto int

AS
BEGIN
 
	SET NOCOUNT ON;
	IF (@municipios =1)
	BEGIN
		select T.NomSector, T.Pctg FROM
(select  top 5 s.NomSector ,  count( p.Id_Proyecto) 
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

select NomSector, Pctg
from
(select top 5 s.NomSector,  count( p.Id_Proyecto)  as Pctg 
from  Proyecto p 
join  SubSector ss on ss.Id_SubSector = p.CodSubSector
join Sector s on s.Id_Sector =ss.CodSector
--where d.Id_Departamento = 25
where p.CodEstado =7
group by  s.NomSector
order by Pctg desc 

union all

select 'Otros', (select   count( p.Id_Proyecto) as Total 
from  Proyecto p 
where p.CodEstado =7)- sum(a.Pctg) from
(select top 5 s.NomSector,   count( p.Id_Proyecto)   as Pctg 
from  Proyecto p 
join  SubSector ss on ss.Id_SubSector = p.CodSubSector
join Sector s on s.Id_Sector =ss.CodSector
--where d.Id_Departamento = 25
where p.CodEstado =7
group by  s.NomSector
order by Pctg desc) a
 ) t
		 END
	IF (@municipios =3)
				BEGIN
				select (
  Select   COUNT(id_empresa) AS NumProyectos
from Empresa
join Ciudad ON Id_Ciudad = CodCiudad
join departamento ON Id_Departamento = CodDepartamento
join Proyecto ON Id_Proyecto= codproyecto
join Estado ON Estado.Id_Estado=proyecto.CodEstado
where CodDepartamento = @iddepto
 
and Id_Estado>=7
and Id_Estado <=9
GROUP BY DEPARTAMENTO.NomDepartamento
 )as Empresa,
 (
  Select COUNT(distinct  EMPRESA.CodCiudad) AS NumMunicipios
from Empresa
join Ciudad ON Id_Ciudad = CodCiudad
join departamento ON Id_Departamento = CodDepartamento
join Proyecto ON Id_Proyecto= codproyecto
join Estado ON Estado.Id_Estado=proyecto.CodEstado
where CodDepartamento = @iddepto 
and Id_Estado>=7
and Id_Estado <=9
GROUP BY DEPARTAMENTO.NomDepartamento
 )as Municipios,
 (
  SELECT SUM (recursos) FROM (
  SELECT e.ValorRecomendado * (SELECT SalarioMinimo FROM dbo.SalariosMinimos WHERE AñoSalario = (select (DatePart(yyyy,c.fechaInicio)))) AS recursos
  FROM Evaluacionobservacion AS e
  INNER JOIN dbo.Convocatoria AS c ON c.Id_Convocatoria = e.CodConvocatoria
  INNER JOIN dbo.Empresa AS p ON p.codproyecto = e.CodProyecto
  INNER JOIN dbo.Ciudad ON p.CodCiudad = Id_Ciudad
  WHERE CodDepartamento = @iddepto) AS r
 )AS resursos,
 (
  SELECT SUM(eg.Total) FROM dbo.EmpleosGenerados AS eg
  INNER JOIN dbo.Empresa AS e ON e.CodProyecto = eg.codproyecto
  INNER JOIN dbo.Ciudad ON e.CodCiudad = Id_Ciudad
  WHERE CodDepartamento = @iddepto
 )AS Empleos, (select d.NomDepartamento from departamento d where d.Id_Departamento=@iddepto) as Departamento
		 END
	IF (@municipios =4)
				BEGIN
				select (
  Select   COUNT(id_empresa) AS NumProyectos
from Empresa
join Ciudad ON Id_Ciudad = CodCiudad
join departamento ON Id_Departamento = CodDepartamento
join Proyecto ON Id_Proyecto= codproyecto
join Estado ON Estado.Id_Estado=proyecto.CodEstado
where  
 
  Id_Estado>=7
and Id_Estado <=9
 
 )as Empresa,
 (
  Select COUNT(distinct  EMPRESA.CodCiudad) AS NumMunicipios
from Empresa
join Ciudad ON Id_Ciudad = CodCiudad
join departamento ON Id_Departamento = CodDepartamento
join Proyecto ON Id_Proyecto= codproyecto
join Estado ON Estado.Id_Estado=proyecto.CodEstado
where  Id_Estado>=7
and Id_Estado <=9
 
 )as Municipios,
 (
  SELECT SUM (recursos) FROM (
  SELECT e.ValorRecomendado * (SELECT SalarioMinimo FROM dbo.SalariosMinimos WHERE AñoSalario = (select (DatePart(yyyy,c.fechaInicio)))) AS recursos
  FROM Evaluacionobservacion AS e
  INNER JOIN dbo.Convocatoria AS c ON c.Id_Convocatoria = e.CodConvocatoria
  INNER JOIN dbo.Empresa AS p ON p.codproyecto = e.CodProyecto
  INNER JOIN dbo.Ciudad ON p.CodCiudad = Id_Ciudad
  ) AS r
 )AS recursos,
 (
  SELECT SUM(eg.Total) FROM dbo.EmpleosGenerados AS eg
  INNER JOIN dbo.Empresa AS e ON e.CodProyecto = eg.codproyecto
  INNER JOIN dbo.Ciudad ON e.CodCiudad = Id_Ciudad
  
 )AS Empleo,'Colombia' as Departamento																		
		 END
END