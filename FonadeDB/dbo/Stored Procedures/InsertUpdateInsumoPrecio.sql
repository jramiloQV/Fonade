CREATE PROCEDURE [dbo].[InsertUpdateInsumoPrecio]
(
@CodigoInsumo int,
@Periodo int,
@Precio float
)
As

if(SELECT count(1) FROM ProyectoInsumoPrecio WHERE CodInsumo = @CodigoInsumo and Periodo=@Periodo)=0
	BEGIN
		INSERT INTO ProyectoInsumoPrecio (CodInsumo,Periodo,Precio) VALUES(@CodigoInsumo,@Periodo,@Precio)
	END
ELSE
	BEGIN
		UPDATE ProyectoInsumoPrecio SET Precio=@Precio WHERE CodInsumo = @CodigoInsumo and Periodo=@Periodo
	END