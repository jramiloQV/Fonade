
create PROCEDURE [dbo].[MD_insert_ProyectoInsumoPrecio]

	@codinsumo int
	,@periodo int
	,@precio money

AS

BEGIN

	INSERT INTO ProyectoInsumoPrecio 
	(
		codinsumo
		,Periodo
		,precio
	)
	VALUES 
	(
		@codinsumo
		,@periodo
		,@precio
	)
		
END