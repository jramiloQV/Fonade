-- =============================================
-- Author:		Alberto Palencia Benedetti
-- Create date: 16/03/2014
-- Description:	Obtener el asesor si es lider o asesor
/* 
[MD_ObtenerAsesor] 1,10404,166
*/
-- =============================================
CREATE PROCEDURE [dbo].[MD_ObtenerAsesor]
	@tipo int ,
	@proyecto int ,
	@convocatoria int
AS
BEGIN
	
	SET NOCOUNT ON;

IF @tipo = 1
	BEGIN
		SELECT 
		  DISTINCT (C.NOMBRES + ' ' + C.APELLIDOS) Nombres
		  ,C.EMAIL
		  ,C.TELEFONO
	FROM CONTACTO C JOIN PROYECTOCONTACTO PC ON (PC.CODROL=1 AND PC.INACTIVO=0 
	AND PC.CODCONTACTO=C.ID_CONTACTO) 
	WHERE PC.CODPROYECTO = @proyecto 
  END
  ELSE IF @tipo = 2
	BEGIN
		SELECT
		   DISTINCT (C.NOMBRES + ' ' + C.APELLIDOS) Nombres
		   ,C.EMAIL, C.TELEFONO
	FROM CONTACTO C JOIN PROYECTOCONTACTO PC ON (PC.CODROL=2 AND PC.INACTIVO=0 
	AND PC.CODCONTACTO=C.ID_CONTACTO AND (PC.ACREDITADOR IS NULL OR PC.ACREDITADOR=0  ))
	WHERE PC.CODPROYECTO = @proyecto 

	END
END