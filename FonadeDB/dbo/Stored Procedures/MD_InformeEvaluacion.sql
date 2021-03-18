-- =============================================
-- Author:		Alberto Palencia Benedetti
-- Create date:  13/03/2014
-- Description:	 Se crea para obtener las consultas que nos serviran para llenar el reporte de evaluacion.
/*
	ejemplo :

	MD_InformeEvaluacion 50161,203

*/
-- =============================================
CREATE PROCEDURE [dbo].[MD_InformeEvaluacion]
	
	@codProyecto int -- codigo del proyecto
	,@codConvocatoria int -- codigo de la convocatoria
	
AS
BEGIN

declare @emprendedor int  = 3
	
	SET NOCOUNT ON;

SELECT        
	C.Nombres + ' ' + C.Apellidos AS Emprendedor
	,E.Entidades
	,E.ValorCartera
	,E.ValorOtrasCarteras
	,ISNULL(E.PeorCalificacion,'') PeorCalificacion
	,E.CuentasCorrientes
FROM EvaluacionContacto AS E INNER JOIN Contacto AS C ON E.CodContacto = C.Id_Contacto 
					INNER JOIN ProyectoContacto AS P ON E.CodProyecto = P.CodProyecto AND C.Id_Contacto = P.CodContacto
WHERE  (P.Inactivo = 0) AND (P.CodRol = @emprendedor) AND (E.CodProyecto = @codproyecto) AND (E.CodConvocatoria = @codConvocatoria)



END