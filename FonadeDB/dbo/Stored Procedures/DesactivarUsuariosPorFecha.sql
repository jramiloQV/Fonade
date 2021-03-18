
/* =============================================
Author:		Wilson Guasca
alter date: 2010-06-21 11:20
Description: Ejecuta desactivación de usuarios, 
  creando además los registros necesarios para 
  el seguimiento en las tablas 
  [ServicioRegistroDesactivacion]
  [ServicioUsuariosDesactivados]
  @FechaReferencia : Fecha de cotejo para comprobación de usuarios
  @FechaInicio : Momento en la que se ejecuta el procedimiento (para registro)

EXECUTE [DesactivarUsuariosPorFecha] '2010-06-05 00:05', '2010-06-21 11:50:38'
============================================= */
CREATE PROCEDURE [dbo].[DesactivarUsuariosPorFecha]
	@FechaReferencia DateTime, 
	@FechaInicio DateTime 
AS
BEGIN

	SET NOCOUNT ON;
	-- Variables de trabajo
	DECLARE @CodRegistro int
	DECLARE @TotalDesactivados int
	DECLARE @FechaPartida DateTime
	DECLARE @FinDesactivacion DateTime
	DECLARE @DatosEjecucion TABLE
		(CodRegistro int NULL, ConteoRegistros int NULL)

	SET @FechaPartida = GetDate()

	-- PREVENIR REPROCESO por una fecha anterior
	IF EXISTS(SELECT TOP 1 [Id_ServicioRegistroDesactivacion] 
			   FROM [ServicioRegistroDesactivacion]
			   WHERE FechaReferencia >= @FechaReferencia)
		RETURN -1;

	INSERT @DatosEjecucion
	EXECUTE InsertaActualizaRegistroDesactivacion
		@FechaReferencia = @FechaReferencia,
		@InicioDesactivacion = @FechaPartida,
		@FinDesactivacion  = NULL,
		@TotalDesactivados = 0,
		@FechaProcesoNotificacion = NULL

	SELECT @CodRegistro = CodRegistro FROM @DatosEjecucion

	IF @CodRegistro IS NULL 
		RETURN -1;

	-- INSERTAR LOS REGISTROS QUE CUMPLEN LAS CONDICIONES
	INSERT INTO [ServicioUsuariosDesactivados]
			   ([CodServicioRegistroDesactivacion]
			   ,[CodContacto]
			   ,[FechaUltimoAcceso]
			   ,[FechaNotificacion]
			   ,[EnvioExitoso])
	SELECT @CodRegistro, C.Id_Contacto , NULL , NULL, 0
	FROM   Contacto AS C INNER JOIN
		   GrupoContacto AS GC ON C.Id_Contacto = GC.CodContacto INNER JOIN
		   Grupo AS G ON GC.CodGrupo = G.Id_Grupo
	WHERE  (C.Inactivo = 0) 
		   AND (G.Id_Grupo IN (5, 6)) 
			AND (ISNULL(fechaCreacion, 0) < @FechaReferencia)
		   AND (C.Id_Contacto NOT IN
		  (SELECT  CodContacto
			FROM   LogIngreso
			WHERE  (FechaUltimoIngreso >= @FechaReferencia)
		   UNION
		   SELECT  CodContacto
			FROM   ContactoReActivacion
			WHERE  (FechaReActivacion >= @FechaReferencia)))
			 AND (C.Id_Contacto NOT IN
				  (SELECT  pc.CodContacto
					FROM   ProyectoContacto AS pc INNER JOIN
						   Proyecto AS p ON pc.CodProyecto = p.Id_Proyecto
					WHERE  (p.CodEstado > 1)
								AND (pc.Inactivo = 0)
								AND (pc.CodRol IN (1, 2, 3))))

	-- ACTUALIZAR ESTADO DE DESACTIVADO A LOS USUARIOS
	UPDATE [Contacto]
	SET    [Inactivo] = 1
	FROM   [ServicioUsuariosDesactivados] AS SUD INNER JOIN
		   [Contacto] AS C ON SUD.CodContacto = C.Id_Contacto
	 WHERE (SUD.CodServicioRegistroDesactivacion = @CodRegistro)
--			AND (SUD.CodServicioRegistroDesactivacion = @CodRegistro)

	-- ACTUALIZAR FECHA DE ULTIMO INGRESO EN EL REGISTRO
	UPDATE [ServicioUsuariosDesactivados]
	SET    [FechaUltimoAcceso] = L.FechaUltimoIngreso
	FROM   ServicioUsuariosDesactivados AS SUD INNER JOIN
		   LogIngreso AS L ON SUD.CodContacto = L.CodContacto
	 WHERE (SUD.CodServicioRegistroDesactivacion = @CodRegistro)

	-- CONTEO DE REGISTROS INSERTADOS
	SELECT @TotalDesactivados = COUNT(1)
	  FROM [ServicioUsuariosDesactivados]
	 WHERE (CodServicioRegistroDesactivacion = @CodRegistro)

	SET @FinDesactivacion = @FechaInicio + (GetDate() - @FechaPartida)

	EXECUTE InsertaActualizaRegistroDesactivacion
		@FechaReferencia = @FechaReferencia,
		@InicioDesactivacion = @FechaInicio,
		@FechaProcesoNotificacion = NULL,
		@TotalDesactivados = @TotalDesactivados,
		@FinDesactivacion  = @FinDesactivacion,
		@CodRegistro = @CodRegistro;

	-- RETORNAR RESULTADOS
	--INSERT @DatosEjecucion
	SELECT Cast(@CodRegistro As int) As 'Id_Registro', 
			Cast(@TotalDesactivados As int) As 'Afectados'

	--SELECT * FROM @DatosEjecucion
END