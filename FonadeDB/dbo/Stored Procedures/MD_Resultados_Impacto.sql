-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [MD_Resultados_Impacto]
	-- Add the parameters for the stored procedure here
	@coddepartamento int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select (
		select COUNT(id_empresa) from Empresa
		inner join Ciudad on CodCiudad = Id_Ciudad
		--INNER JOIN dbo.departamento ON CodDepartamento = Id_Departamento
		where CodDepartamento = @coddepartamento
	)as Empresa,
	(
		select COUNT(id_ciudad) from Ciudad
		where CodDepartamento = @coddepartamento
	)as Municipios,
	(
		SELECT SUM (recursos) FROM (
		SELECT e.ValorRecomendado * (SELECT SalarioMinimo FROM dbo.SalariosMinimos WHERE AñoSalario = (select (DatePart(yyyy,c.fechaInicio)))) AS recursos
		FROM Evaluacionobservacion AS e
		INNER JOIN dbo.Convocatoria AS c ON c.Id_Convocatoria = e.CodConvocatoria
		INNER JOIN dbo.Empresa AS p ON p.codproyecto = e.CodProyecto
		INNER JOIN dbo.Ciudad ON p.CodCiudad = Id_Ciudad
		WHERE CodDepartamento = @coddepartamento) AS r
	)AS resursos,
	(
		SELECT SUM(eg.Total) FROM dbo.EmpleosGenerados AS eg
		INNER JOIN dbo.Empresa AS e ON e.CodProyecto = eg.codproyecto
		INNER JOIN dbo.Ciudad ON e.CodCiudad = Id_Ciudad
		WHERE CodDepartamento = @coddepartamento
	)AS Empleos
END