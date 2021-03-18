/*
MD_InsertUpdateDeleteProyectoInsumoPrecio 0,59202,1,'natilla',34.88,'METROS','frasco','344.4','Create','144499.92','78',105406
*/
CREATE PROCEDURE [dbo].[MD_InsertUpdateDeleteProyectoInsumoPrecio]
	@CodInsumo int
	,@codProyecto int
	,@Tipo int
	,@NomInsumo varchar(100)
	,@IVA VARCHAR(5)
	,@Unidad varchar(15)
	,@Presentacion varchar(30)
	,@Credito varchar
	,@caso varchar(10)
	,@Cantidad varchar(10)
	,@Desperdicio varchar(10)
	,@CodProducto varchar(10)
AS
BEGIN
	IF @caso='Create'
		begin
		declare @VersionId uniqueidentifier=NEWID()
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
				,VersionId
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
				,@VersionId
			)

			/*CodProducto	CodInsumo	Presentacion	Cantidad	Desperdicio
595	461	bolsas	1	16*/
			set @CodInsumo=(select Id_Insumo from  ProyectoInsumo where VersionId=@VersionId)
				if isnull(@CodInsumo,0)>0
				begin
					INSERT INTO ProyectoProductoInsumo(CodProducto,	CodInsumo,Presentacion,	Cantidad,	Desperdicio)
					VALUES(@CodProducto,@CodInsumo,@Presentacion,	@Cantidad,	@Desperdicio)
					
				end
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
					select  Id_Insumo from  ProyectoInsumo where Id_Insumo= @CodInsumo
	
END