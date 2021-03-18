/*
-- Author:		alberto palencia Benedetti
-- Create date: 12/03/2014
-- Description:	Obtengo la informacion de las ciuades para imprimir el reporte de evaluacion
MD_ObtenerInformacionEvaluacion  50161
 */
CREATE PROCEDURE  MD_ObtenerInformacionEvaluacion
	-- Add the parameters for the stored procedure here
	@codProyecto int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

  
	SELECT  cast(@codProyecto as varchar(50)) + ' - ' + nomProyecto AS nomProyecto
	, Sumario, nomCiudad + ' - ' + nomDepartamento Ciudad, nomUnidad + ' - ' + nomInstitucion Unidad
	FROM Proyecto P, Ciudad C, Departamento D, institucion I
	WHERE P.codCiudad = C.id_Ciudad AND
	C.codDepartamento = D.id_Departamento AND P.codInstitucion=I.id_Institucion AND
	id_Proyecto = @codProyecto

END