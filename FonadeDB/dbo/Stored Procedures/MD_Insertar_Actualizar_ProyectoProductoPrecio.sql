-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_Insertar_Actualizar_ProyectoProductoPrecio]
	-- Add the parameters for the stored procedure here
		@_CodProducto int,
		@_Periodo tinyint,
		@_Precio char(10),
		@_caso varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @_caso = 'CREATE'
		BEGIN
			INSERT INTO [ProyectoProductoPrecio]
				([CodProducto],[Periodo],[Precio])
			VALUES
				(@_CodProducto,@_Periodo,@_Precio)
		END
	IF @_caso = 'UPDATE'
		BEGIN
			--DELETE FROM [ProyectoProductoPrecio] WHERE EXISTS(SELECT [CodProducto] FROM [ProyectoProductoPrecio] WHERE [CodProducto] = @_CodProducto)
			
			--INSERT INTO [ProyectoProductoPrecio]
			--	([CodProducto],[Periodo],[Precio])
			--VALUES
			--	(@_CodProducto,@_Periodo,@_Precio)

				update ProyectoProductoPrecio
				set Precio = @_Precio
				where CodProducto = @_CodProducto
				and Periodo = @_Periodo
		END
END