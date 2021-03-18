-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [MD_Insertar_Actualizar_ProyectoProducto]
	-- Add the parameters for the stored procedure here
			@_Id_Producto int,
			@_CodProyecto int,
			@_NomProducto varchar(255),
			@_PorcentajeIva float,
			@_PorcentajeRetencion float,
			@_PorcentajeVentasContado float,
			@_PorcentajeVentasPlazo float,
			@_PosicionArancelaria char(10),
			@_PrecioLanzamiento money,
			@_caso varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @_caso = 'CREATE'
		BEGIN
			INSERT INTO [ProyectoProducto]
				([CodProyecto],[NomProducto],[PorcentajeIva],[PorcentajeRetencion],[PorcentajeVentasContado],[PorcentajeVentasPlazo],[PosicionArancelaria],[PrecioLanzamiento])
			VALUES
				(@_CodProyecto,@_NomProducto,@_PorcentajeIva,@_PorcentajeRetencion,@_PorcentajeVentasContado,@_PorcentajeVentasPlazo,@_PosicionArancelaria,@_PrecioLanzamiento)
		END
	IF @_caso = 'UPDATE'
	BEGIN
		UPDATE [ProyectoProducto]
		SET
		  [NomProducto] = @_NomProducto
		  ,[PorcentajeIva] = @_PorcentajeIva
		  ,[PorcentajeRetencion] = @_PorcentajeRetencion
		  ,[PorcentajeVentasContado] = @_PorcentajeVentasContado
		  ,[PorcentajeVentasPlazo] = @_PorcentajeVentasPlazo
		  ,[PosicionArancelaria] = @_PosicionArancelaria
		  ,[PrecioLanzamiento] = @_PrecioLanzamiento
		WHERE [Id_Producto] = @_Id_Producto
	END
END