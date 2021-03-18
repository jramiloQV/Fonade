/*
-- Author:		Alberto Palencia Benedetti
-- Create date: 17/03/2014
-- Description:	Obtengo el listado del proyecto de acreditacion, se hace el query dinamico porque puede buscar por varios parametros.
ejemplo: 
  MD_ObtenerListadoProyectoProcesoAcreditacion 0,0,'PRODUCCION DE TOMATE LIMPIO LA FORTUNA',0,'','',''

*/

CREATE PROCEDURE	[dbo].[MD_ObtenerListadoProyectoProcesoAcreditacion]
	@CodConvocatoria int
	,@CodProyecto int
	,@NomProyecto NVarchar(1000)
	,@Estado  INT
	,@NombreEmprendedor NVarchar(1000)
	,@ApellidosEmprendedor NVarchar(1000)
	,@DocumentoEmprendedor INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


DECLARE @sql nVARCHAR(4000) , @WHERE nVARCHAR(3000)

Set @WHERE =  Char(10)

/*  WHERE DINAMICO DEPENDIENDO DE LA INFORMACION QUE INGRESEN */

IF  @CodConvocatoria <> 0 AND @CodConvocatoria IS NOT NULL
	BEGIN
			SET	@WHERE =  @WHERE + ' and CO.Id_Convocatoria = ' + CAST(@CodConvocatoria AS Nvarchar(100)) + ''
    END

IF  @CodProyecto <> 0 AND @CodProyecto IS NOT NULL
	BEGIN
		SET	@WHERE = @WHERE  + ' and PR.Id_Proyecto = ' +  CAST(@CodProyecto AS Nvarchar(100)) + ''
    END


IF  @NomProyecto <> '' AND @NomProyecto IS NOT NULL
	BEGIN
		SET	@WHERE = @WHERE  + ' and PR.NomProyecto like ''' + @NomProyecto + '%''' + ''
    END 
	
IF  @Estado <> '' AND @Estado IS NOT NULL
	BEGIN
		SET	@WHERE = @WHERE  + ' and PR.CodEstado =' + CAST(@Estado AS VARCHAR(10))  + ''
    END 
	
	IF  @NombreEmprendedor <> '' AND @NombreEmprendedor IS NOT NULL
	BEGIN
		SET	@WHERE = @WHERE  + ' and EM.Nombres like ''' +  @NombreEmprendedor +  '%''' + ''
    END

   IF  @ApellidosEmprendedor <> '' AND @ApellidosEmprendedor IS NOT NULL
	BEGIN
		SET	@WHERE = @WHERE  + ' and EM.Apellidos like ''' + @ApellidosEmprendedor  + '%''' +''
    END

   IF  @DocumentoEmprendedor <> '' AND @DocumentoEmprendedor IS NOT NULL
	BEGIN
		SET	@WHERE = @WHERE  + ' and PAD.CodProyecto =' + CAST(@DocumentoEmprendedor AS VARCHAR(20))  + ''
    END


/* FIN DEL WHERE DINAMICO */

SET @sql = 'SELECT DISTINCT
			 CO.Id_Convocatoria as CodConvocatoria, CO.NomConvocatoria, PR.Id_Proyecto as CodProyecto, PR.NomProyecto
			,PC.Acreditador
			-- , C.Nombres AS NombreAcreditador, C.Apellidos AS ApellidosAcreditado
			, PA.Fecha, PA.CodEstado, E.NomEstado, EM.Nombres AS NombreEmprendedor, EM.Email
			, EM.Apellidos AS ApellidosEmprendedor
			
		 FROM Proyecto AS PR
		 INNER JOIN ConvocatoriaProyecto as CP ON CP.CodProyecto = PR.Id_Proyecto
		 INNER JOIN Convocatoria AS CO ON CO.Id_Convocatoria = CP.CodConvocatoria
		 LEFT OUTER JOIN Estado AS E ON PR.CodEstado = E.Id_Estado
		 LEFT OUTER JOIN ProyectoContacto AS PC ON PR.Id_Proyecto = PC.CodProyecto and PC.Acreditador = 1 and PC.Inactivo = 0 and PC.FechaFin is null
		 LEFT OUTER JOIN Contacto AS C ON PC.CodContacto = C.Id_Contacto
		 LEFT OUTER JOIN ProyectoAcreditacionUltimo as PA ON CO.Id_Convocatoria = PA.CodConvocatoria and PR.Id_Proyecto = PA.CodProyecto
		 LEFT OUTER JOIN ProyectoAcreditacionDocumento AS PAD ON PA.CodConvocatoria =  PAD.CodConvocatoria and PA.CodProyecto = PAD.CodProyecto
		 LEFT OUTER JOIN Contacto AS EM ON PAD.CodContacto = EM.Id_Contacto
		 WHERE CO.Id_Convocatoria is not null ' + @WHERE +  '  ORDER BY CodConvocatoria, CodProyecto'

  EXEC SP_EXECUTESQL  @sql;

   
END