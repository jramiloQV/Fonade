-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
/*
exec MD_Insertar_Actualizar_ProductosVentas 295843,59251, 'kjkjkjkjk',12.9,23.4,56.89,'', 9999999
*/
CREATE PROCEDURE [dbo].[MD_Insertar_Actualizar_ProductosVentas]
	-- Add the parameters for the stored procedure here
			@_Id_Producto int,
			@_CodProyecto int,
			@_NomProducto varchar(255),
			@_PorcentajeIva float,
			@_PorcentajeRetencion float,
			@_PorcentajeVentasPlazo float,
			@_PosicionArancelaria char(10),
			@_PrecioLanzamiento money
AS
BEGIN
declare 		@_PorcentajeVentasContado float
	set @_PorcentajeVentasContado=100-@_PorcentajeVentasPlazo
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	    -- Insert statements for procedure here
		IF @_Id_Producto>0
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
				SELECT 'true' + '-'+  convert(varchar(10),@_Id_Producto) 
			
		END
		ELSE
		BEGIN			
			IF(SELECT COUNT(1) FROM  ProyectoProducto WHERE NomProducto = @_NomProducto AND CodProyecto=@_CodProyecto )=0
				BEGIN
					INSERT INTO [ProyectoProducto]
						([CodProyecto],[NomProducto],[PorcentajeIva],[PorcentajeRetencion],[PorcentajeVentasContado],[PorcentajeVentasPlazo],[PosicionArancelaria],[PrecioLanzamiento])
					VALUES
						(@_CodProyecto,@_NomProducto,@_PorcentajeIva,@_PorcentajeRetencion,@_PorcentajeVentasContado,@_PorcentajeVentasPlazo,@_PosicionArancelaria,@_PrecioLanzamiento)
					SELECT TOP 1 'true' + '-'+  convert(varchar(10),Id_Producto) from ProyectoProducto WHERE NomProducto = @_NomProducto AND CodProyecto=@_CodProyecto 
				END
			ELSE
			BEGIN
				SELECT 'false' + '-'+  convert(varchar(10),@_Id_Producto) 
			END
			END
END