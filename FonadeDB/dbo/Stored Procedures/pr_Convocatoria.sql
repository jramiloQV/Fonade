


CREATE   PROCEDURE pr_Convocatoria
	@id_Convocatoria	int = null,	--Convocatoria a procesar
	@id_Proyecto		int = null	--Proyecto a procesar

/*Procedimiento para convocatoria de proyectos*/
AS
BEGIN
	DECLARE	@codEstadoFormalizado int,
		@codEstadoConvocado int,
		@codConvocatoria int	 --convocatoria a la que se agrega el proyecto
	/*VARIABLES PARA EL CURSOR*/
	DECLARE @codProyecto int,
		@codCiudad int,
		@codDepartamento int, 
		@codSector int

	/*inicia parámetros*/
	SET @codEstadoFormalizado = 2	--Estado valido para ser convocado = 'Aprobación Técnica'
	SET @codEstadoConvocado   = 3	--Estado convocado = 'Convocatoria'

	/*selecciona los proyectos*/
	IF isnull(@id_Proyecto, 0) <> 0	--Se recibió el parametro @id_proyecto, un unico proyecto a convocar
		DECLARE CR_Proyectos CURSOR FOR
		SELECT P.id_Proyecto, S.codSector, P.codCiudad, C.codDepartamento
		FROM Proyecto P, SubSector S, Ciudad C, Departamento D
		WHERE 	P.codCiudad = C.id_Ciudad AND 
			C.codDepartamento = D.id_Departamento AND 
			S.id_Subsector = P.codSubsector AND 
			Inactivo = 0 AND P.codEstado = @codEstadoFormalizado AND
			P.id_Proyecto = @id_Proyecto AND --el proyecto específico
			3 > (SELECT COUNT(1) FROM ConvocatoriaProyecto C WHERE C.codProyecto = P.id_Proyecto)
	ELSE	--Todos los proyectos que puedan ser convocados
		DECLARE CR_Proyectos CURSOR FOR
		SELECT P.id_Proyecto, S.codSector, P.codCiudad, C.codDepartamento
		FROM Proyecto P, SubSector S, Ciudad C, Departamento D
		WHERE 	P.codCiudad = C.id_Ciudad AND 
			C.codDepartamento = D.id_Departamento AND 
			S.id_Subsector = P.codSubsector AND 
			Inactivo = 0 AND P.codEstado = @codEstadoFormalizado AND
			3 > (SELECT COUNT(1) FROM ConvocatoriaProyecto C WHERE C.codProyecto = P.id_Proyecto)	

	OPEN CR_Proyectos
	
	FETCH NEXT FROM CR_Proyectos
	INTO @codProyecto, @codSector, @codCiudad, @codDepartamento

	WHILE @@FETCH_STATUS = 0
	BEGIN 	/*	--------------------------
			PRIORIDAD DE CALIFICACIÓN
			DEPTO	CIUDAD	SECTOR
			DEPTO	CIUDAD	(TODO)
			DEPTO	(TODO)	SECTOR
			DEPTO	(TODO)	(TODO)
			(TODO)	(TODO)	SECTOR
			(TODO)	(TODO)	(TODO)
			--------------------------	*/
		SET @CodConvocatoria=0
		--Toma solo la primera coincidencia
		IF isnull(@id_convocatoria, 0) <> 0 -- si se recibió el parametro @id_convocatoria
			SELECT TOP 1 @codConvocatoria = K.codConvocatoria
			FROM 	ConvocatoriaCriterioCiudad KC, ConvocatoriaCriterioSector KS, ConvocatoriaCriterio K, Convocatoria F
			WHERE  	F.id_Convocatoria = K.codConvocatoria AND 
				getdate() between F.fechaInicio AND F.fechaFin AND
				KC.codCriterio=K.id_criterio AND
				KS.codCriterio=K.id_criterio AND 
				F.Publicado=1 AND --Debe estar publicada 
				K.codConvocatoria=@id_convocatoria AND --La convocatoria específica
				KC.codDepartamento in(0, @codDepartamento)  AND KS.codSector in(0,@CodSector) AND codCiudad in(0,@CodCiudad)
			ORDER BY KC.codCiudad DESC, KC.codDepartamento DESC, KS.codSector desc
		ELSE
			SELECT TOP 1 @codConvocatoria = K.codConvocatoria
			FROM 	ConvocatoriaCriterioCiudad KC, ConvocatoriaCriterioSector KS, ConvocatoriaCriterio K, Convocatoria F
			WHERE  	F.id_Convocatoria = K.codConvocatoria AND 
				getdate() between F.fechaInicio AND F.fechaFin AND
				KC.codCriterio=K.id_criterio AND 
				KS.codCriterio=K.id_criterio AND 
				F.Publicado=1 AND
				KC.codDepartamento in(0, @codDepartamento)  AND KS.codSector in(0,@CodSector) AND codCiudad in(0,@CodCiudad)
			ORDER BY KC.codCiudad DESC, KC.codDepartamento DESC, KS.codSector desc
		
		--Si el proyecto coincidió con alguna convocatoria 
		IF isnull(@codConvocatoria, 0)<> 0
		BEGIN
			--Agrega el proyecto a la convocatoria
			INSERT INTO ConvocatoriaProyecto (codConvocatoria, codProyecto, Fecha) VALUES(@codConvocatoria, @codProyecto, getDate())
			
			-- Cambia el estado del proyecto a 'Convocatoria'
			UPDATE Proyecto
			SET codEstado=@codEstadoConvocado 
			WHERE id_Proyecto=@codProyecto
			
			-- Asignar convocatoria a la formalizacion del proyecto
			UPDATE ProyectoFormalizacion set codconvocatoria=@codconvocatoria
			where codproyecto=@CodProyecto and codconvocatoria is null			

			--Adicionar indicadores de evaluacion
			insert into evaluacionproyectoindicador (codproyecto,codconvocatoria,descripcion,tipo,valor,protegido)
			select @codProyecto,@codConvocatoria,descripcion,tipo,0,1 from indicadormodelo
			
			--Adicionar supuestos de evaluacion
			insert into evaluacionproyectosupuesto (nomevaluacionProyectosupuesto,codtiposupuesto,codproyecto,codconvocatoria)
			select nomevaluacionproyectosupuesto,codtiposupuesto,@codProyecto,@codConvocatoria 
			from evaluacionproyectosupuestomodelo where inactivo=0
			
			--Adicionar indicadores financieros
			insert into EvaluacionIndicadorFinancieroProyecto (descripcion,codproyecto,codconvocatoria)
			select descripcion, @codproyecto, @codConvocatoria 
			from EvaluacionIndicadorFinancieromodelo where inactivo=0

			--Adicionar rubro
			insert into evaluacionrubroproyecto (descripcion,codproyecto,codconvocatoria)
			select descripcion, @codproyecto, @codConvocatoria 
			from evaluacionrubromodelo where inactivo=0
			
			--Adicionar items para desempeño del evaluador
			insert into evaluacionevaluador (codproyecto, codconvocatoria, coditem)
			select @codproyecto, @codconvocatoria, id_item from item

		END

		FETCH NEXT FROM CR_Proyectos
		INTO @codProyecto, @codSector, @codCiudad, @codDepartamento
	END 
	CLOSE CR_Proyectos
	DEALLOCATE CR_Proyectos
END