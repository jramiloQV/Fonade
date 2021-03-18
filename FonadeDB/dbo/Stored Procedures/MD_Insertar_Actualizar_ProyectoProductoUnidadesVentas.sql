-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_Insertar_Actualizar_ProyectoProductoUnidadesVentas]
	-- Add the parameters for the stored procedure here
	@_CodProducto int,
	@_Unidades float,
	@_Mes smallint,
	@_Ano smallint,
	@_caso varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @_caso = 'CREATE'
		BEGIN
			INSERT INTO [ProyectoProductoUnidadesVentas]
				([CodProducto],[Unidades],[Mes],[Ano])
			VALUES
				(@_CodProducto,@_Unidades,@_Mes,@_Ano)
		END
	IF @_caso = 'UPDATE'
		BEGIN
			--DELETE FROM [ProyectoProductoUnidadesVentas] WHERE EXISTS(SELECT [CodProducto] FROM [ProyectoProductoUnidadesVentas] WHERE [CodProducto] = @_CodProducto)
			
			--INSERT INTO [ProyectoProductoUnidadesVentas]
			--	([CodProducto],[Unidades],[Mes],[Ano])
			--VALUES
			--	(@_CodProducto,@_Unidades,@_Mes,@_Ano)

				update ProyectoProductoUnidadesVentas
				set Unidades = @_Unidades
				where CodProducto = @_CodProducto
				and Mes = @_Mes
				and Ano = @_Ano
		END
END