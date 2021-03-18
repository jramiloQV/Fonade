/*
MD_Insert_Update_Delete_ProyectoInsumo 0,56187,1,'natilla',34,'METROS','frasco','3444','Create','14442','23',105406
*/
CREATE PROCEDURE [dbo].[MD_Insert_Update_Delete_ProyectoInsumo]
	@CodInsumo int
	,@codProyecto int
	,@Tipo int
	,@NomInsumo varchar(100)
	,@IVA float
	,@Unidad varchar(15)
	,@Presentacion varchar(30)
	,@Credito float
	,@caso varchar(10)
AS
BEGIN
	IF @caso='Create'
		begin
		INSERT INTO ProyectoInsumo
			(
				CodProyecto
				,codTipoInsumo
				,NomInsumo
				,IVA
				,Unidad
				,Presentacion
				,CompraContado
				,CompraCredito
			) 

			VALUES
			(
				@codProyecto
				,@Tipo
				,@NomInsumo
				,@IVA
				, @Unidad
				,@Presentacion
				,100-@Credito
				,@Credito

			)
		end
	IF @caso='Update'
	begin
		UPDATE ProyectoInsumo 
			SET 
				NomInsumo = @NomInsumo
				,Unidad = @Unidad
				,codTipoInsumo = @Tipo
				,IVA = @IVA
				,Presentacion = @Presentacion
				,CompraContado = 100-@Credito
				,CompraCredito = @Credito
			WHERE Id_Insumo = @CodInsumo

			DELETE FROM ProyectoInsumoPrecio WHERE codinsumo = @CodInsumo
		
		end

	IF @caso='Delete'

		begin

			Delete from ProyectoEmpleoManoObra where codmanoobra=@CodInsumo
			DELETE FROM ProyectoInsumoUnidadesCompras WHERE codInsumo=@CodInsumo
			DELETE FROM ProyectoInsumoPrecio WHERE codInsumo=@CodInsumo
			DELETE FROM ProyectoProductoInsumo WHERE codInsumo=@CodInsumo
			DELETE FROM ProyectoInsumo WHERE id_Insumo=@CodInsumo

		end

END