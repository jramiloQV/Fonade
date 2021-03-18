-- =============================================
-- Author:		Alberto Palencia Benedetti
-- Create date: 27/03/2014
-- Description:	carga la informacion de las tablas dinamicas.
--  MD_GenerarInformeEvaluacionTablasDinamicas 50161,203,5
-- =============================================
CREATE PROCEDURE  [dbo].[MD_GenerarInformeEvaluacionTablasDinamicas]
	-- Add the parameters for the stored procedure here
	@codproyecto int
	,@codconvocatoria int 
	,@cantidad int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @contador INT = 1,  @colums NVARCHAR(1000)
	, @query Nvarchar(max)
	, @query1 Nvarchar(max)
	, @query2 Nvarchar(max)

		SET @colums = '';

		WHILE (@contador <= @cantidad)
		BEGIN
			SET @colums += ' , SUM( CASE isnull(periodo, 0) when ' + cast(@contador AS NVARCHAR(100)) 
				+  ' then isnull(V.Valor, 0) else 0 end) Valor' + cast(@contador AS NVARCHAR(100))
			SET @contador +=1;
		END


	SET @query = 'SELECT T.nomTipoSupuesto tipo, S.nomEvaluacionProyectoSupuesto Nombre ' +
                     @colums + ', SUM( CASE isnull(periodo, 0) when 0 then isnull(V.Valor, 0) else 0 end) Promedio ' +
                     ' FROM evaluacionProyectoSupuesto S ' +
                     'INNER JOIN TipoSupuesto T ON T.id_TipoSupuesto = S.codTipoSupuesto '  +
                     'LEFT JOIN  evaluacionProyectoSupuestoValor V ON S.Id_EvaluacionProyectoSupuesto = V.codSupuesto ' +
                     'WHERE S.codProyecto= ' + cast(@codproyecto as Nvarchar(300)) + 
					 ' and codConvocatoria = ' + cast(@codconvocatoria as Nvarchar(300)) + ' ' +
                     'GROUP BY T.nomTipoSupuesto, S.nomEvaluacionProyectoSupuesto ' +
                    'ORDER BY T.nomTipoSupuesto, S.nomEvaluacionProyectoSupuesto ';

      SET @query1  =   '         SELECT S.Descripcion Nombre ' +
										 @colums +
                                ' FROM EvaluacionIndicadorFinancieroProyecto S  ' +
                                'LEFT JOIN  EvaluacionIndicadorFinancieroValor V ON S.Id_EvaluacionIndicadorFinancieroProyecto = V.codEvaluacionIndicadorFinancieroProyecto ' +
								'WHERE S.codProyecto=' + cast(@codproyecto as Nvarchar(300))  +
								' and codConvocatoria = ' + cast(@codconvocatoria as Nvarchar(300))  
								+ ' GROUP BY S.Descripcion ' +
								'ORDER BY S.Descripcion   ';

        SET @query2  =    '     SELECT S.Descripcion Nombre ' +
								@colums + '   FROM EvaluacionRubroProyecto S ' +
									'LEFT JOIN EvaluacionRubroValor V ON S.Id_EvaluacionRubroProyecto = V.codEvaluacionRubroProyecto ' +
								'WHERE S.codProyecto=' +  cast(@codproyecto as Nvarchar(300))
								 +' and codConvocatoria = ' + cast(@codconvocatoria as Nvarchar(300))  + ' ' +
                                'GROUP BY S.Descripcion ' +
                                'ORDER BY S.Descripcion     ';

	PRINT @QUERY


	 SET @query = @query +  @query1 +  @query2
      EXECUTE sp_executesql @query
 
 --  tabla  observaciones ----

	SELECT 
	 descripcion
	,CASE WHEN  Tipo = '$' THEN   '$ ' + CONVERT(VARCHAR(1000),  CAST(VALOR AS money),1)  ELSE
	 CASE WHEN  Tipo = '%' THEN  CONVERT(VARCHAR(1000),  CAST(VALOR AS money),1) + ' %'  ELSE
	 CASE WHEN TIPO = '#' THEN CONVERT(VARCHAR(1000),  CAST(valor AS money),1)  end END END Valor
	FROM EvaluacionProyectoIndicador 
	WHERE codProyecto = @codproyecto
	AND codConvocatoria = @codconvocatoria

	-- tabla de riesgo -- 
	SELECT 
			Riesgo
			,Mitigacion
   FROM EvaluacionRiesgo
   WHERE codProyecto = @codproyecto
   And codConvocatoria= @codconvocatoria
   ORDER BY id_Riesgo


END