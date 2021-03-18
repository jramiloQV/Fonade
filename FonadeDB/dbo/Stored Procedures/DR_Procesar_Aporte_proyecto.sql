CREATE PROCEDURE dbo.DR_Procesar_Aporte_proyecto 
(
	@AporteID INT,
	@CodProyecto INT,
	@Nombre VARCHAR(100),
	@Valor MONEY,
	@Tipo VARCHAR(30),
	@Detalle VARCHAR(1000),
	@Editar BIT = 0
)
AS
BEGIN
	IF(@Editar = 0)
	BEGIN
		INSERT INTO proyectoaporte 
		(
			CodProyecto,
			Nombre,
			Valor,
			TipoAporte,
			Detalle
		)
		VALUES
		(			
			@CodProyecto,
			@Nombre,
			@Valor,
			@Tipo,
			@Detalle
		)	
	
	END
	ELSE
	BEGIN
		UPDATE
			proyectoaporte
		SET
			Nombre = @Nombre,
			Valor = @Valor,
			TipoAporte =  @Tipo,
			Detalle = @Detalle
		WHERE 
			Id_Aporte = @AporteID 
	END
END