-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_Insertar_Actualizar_ProyectoMercadoProyeccionVentas]
	-- Add the parameters for the stored procedure here
		@_CodProyecto int,
		@_FechaArranque smalldatetime,
		@_CodPeriodo tinyint,
		@_TiempoProyeccion tinyint,
		@_MetodoProyeccion varchar(100),
		@_PoliticaCartera text,
		@_CostoVenta varchar(100),
		@_justificacion text,
		@_caso varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @_caso='CREATE'
		BEGIN
			INSERT INTO [dbo].[ProyectoMercadoProyeccionVentas]
			([CodProyecto],[FechaArranque],[CodPeriodo],[TiempoProyeccion],[MetodoProyeccion],[PoliticaCartera],[CostoVenta],[justificacion])
			VALUES
			(@_CodProyecto,@_FechaArranque,@_CodPeriodo,@_TiempoProyeccion,@_MetodoProyeccion,@_PoliticaCartera,@_CostoVenta,@_justificacion)
		END

	IF @_caso='UPDATE'
		BEGIN
			UPDATE [dbo].[ProyectoMercadoProyeccionVentas]
			   SET 
				   [FechaArranque] = @_FechaArranque
				  ,[CodPeriodo] = @_CodPeriodo
				  ,[TiempoProyeccion] = @_TiempoProyeccion
				  ,[MetodoProyeccion] = @_MetodoProyeccion
				  ,[PoliticaCartera] = @_PoliticaCartera
				  ,[CostoVenta] = @_CostoVenta
				  ,[justificacion] = @_justificacion
			 WHERE [CodProyecto] = @_CodProyecto
		END
END