-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [MD_Insertar_Actualizar]
	-- Add the parameters for the stored procedure here
	@_CodProyecto int
	,@_NomProducto varchar(255)
	,@_PorcentajeIva float
	,@_PorcentajeRetencion float
	,@_PorcentajeVentasContado float
	,@_PorcentajeVentasPlazo float
	,@_PosicionArancelaria char(10)
	,@_PrecioLanzamiento money,
	@_caso varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @_caso='CREATE'
		BEGIN
		INSERT INTO [dbo].[ProyectoProducto]([CodProyecto],[NomProducto],[PorcentajeIva],[PorcentajeRetencion],[PorcentajeVentasContado],[PorcentajeVentasPlazo],[PosicionArancelaria],[PrecioLanzamiento])
     VALUES
           (@_CodProyecto
           ,@_NomProducto
           ,@_PorcentajeIva
           ,@_PorcentajeRetencion
           ,@_PorcentajeVentasContado
           ,@_PorcentajeVentasPlazo
           ,@_PosicionArancelaria
           ,@_PrecioLanzamiento)
	END
END