-- =============================================
-- Author:		Alberto Palencia Benedetti
-- Create date: <Create Date,,>
-- Description:	 Carga la informacion del asesor, lider, etc
-- =============================================
CREATE PROCEDURE	[dbo].[MD_ObtenerInformacionProyectoAcreditacion]
  
	@codproyecto int
	,@codconvocatoria int
	,@codusuario int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- 0

	SELECT TOP 1 OBSERVACIONFINAL 
	FROM PROYECTOACREDITACION 
	WHERE  CODCONVOCATORIA = @codconvocatoria
	AND CODPROYECTO = @codproyecto
	 AND CODESTADO=  (SELECT TOP 1  CODESTADO
	FROM PROYECTOACREDITACION 
	WHERE CODPROYECTO = @codproyecto
	AND CODCONVOCATORIA= @codconvocatoria
	ORDER BY FECHA DESC) AND  OBSERVACIONFINAL <> '' ORDER BY FECHA DESC 


/*********************************  fecha del proyecto ****************************************/
	SELECT TOP 1  CODESTADO,FECHA 
	FROM PROYECTOACREDITACION 
	WHERE CODPROYECTO = @codproyecto
	AND CODCONVOCATORIA= @codconvocatoria
	ORDER BY FECHA DESC
/**********************************************************************/

/*********************************  carga la informacion del remitente para enviar correo tabla 2 ****************************************/


		SELECT C.NOMBRES as NombreCorreo
		, C.APELLIDOS as ApellidoCorreo
		, C.EMAIL as Correo
		, C.CARGO As CargoCorreo
		, C.TELEFONO TelefonoCorreo
		, C.DIRECCION DireccionCorreo
		, I.NOMINSTITUCION InstitucionCorreo
		 FROM CONTACTO C LEFT JOIN INSTITUCION I ON I.ID_INSTITUCION = C.CODINSTITUCION
		 WHERE ID_CONTACTO = @codusuario


  /*********************************************************************************** */

/*********************************  carga la informacion de la ciudad del departamento tabla 3 ****************************************/

		SELECT P.NOMPROYECTO
		,C.NOMCIUDAD + '(' + D.NOMDEPARTAMENTO + ')' as NOMCIUDAD
		, I.NOMUNIDAD + ' - ' + I.NOMINSTITUCION as UnidadEmprendimiento
	FROM PROYECTO P
		JOIN CIUDAD C ON (C.ID_CIUDAD = P.CODCIUDAD)
		JOIN DEPARTAMENTO D ON (D.ID_DEPARTAMENTO = C.CODDEPARTAMENTO)
		JOIN INSTITUCION I ON (P.CODINSTITUCION= I.ID_INSTITUCION)
	WHERE P.ID_PROYECTO = @codproyecto

  /*********************************************************************************** */

/* obtiene el nombre del asesor Lider tabla 4  */

	SELECT DISTINCT (NOMBRES + ' ' + APELLIDOS) as NombreLider
	 FROM CONTACTO C JOIN PROYECTOCONTACTO PC 
	 ON (PC.CODROL=1 AND PC.INACTIVO=0 AND PC.CODCONTACTO=C.ID_CONTACTO) 
	 WHERE PC.CODPROYECTO = @codproyecto

 /******************************************************************************************* */

 /* obtiene el nombre del asesor  tabla 5  */
  SELECT DISTINCT (NOMBRES + ' ' + APELLIDOS)  NombreAsesor
		  FROM CONTACTO C JOIN PROYECTOCONTACTO PC 
		  ON (PC.CODROL=2 AND PC.INACTIVO=0 AND PC.CODCONTACTO=C.ID_CONTACTO 
		  AND (PC.ACREDITADOR IS NULL OR PC.ACREDITADOR=0)  ) WHERE PC.CODPROYECTO = @codproyecto

/******************************************************************************************* */

/* UN SOLO CRIF  TABLA 6 */

	SELECT TOP 1 CRIF
	FROM ProyectoAcreditaciondocumentosCRIF
	WHERE CODPROYECTO= @codproyecto
	AND CODCONVOCATORIA = @codconvocatoria
	ORDER BY FECHA DESC

/******************************************************************************************* */

/* TODOS LOS CRIF  tabla 7 */

	SELECT CRIF, FECHA
	FROM ProyectoAcreditaciondocumentosCRIF
	WHERE CODPROYECTO= @codproyecto
	AND CODCONVOCATORIA = @codconvocatoria
	ORDER BY FECHA DESC



   /******************************************************************************************* */

END