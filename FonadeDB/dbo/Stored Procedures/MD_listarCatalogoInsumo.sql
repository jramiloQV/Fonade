create PROCEDURE [dbo].[MD_listarCatalogoInsumo]
	@proyecto int,
	@tipoinsumo int,
	@producto int

AS

BEGIN

	IF @tipoinsumo=0

		begin

			SELECT id_Insumo
			, nomTipoInsumo
			, nomInsumo
			, Presentacion
			, Unidad
			, id_TipoInsumo
			FROM ProyectoInsumo, TipoInsumo
			WHERE codTipoInsumo = id_TipoInsumo 
			AND codProyecto = @proyecto
			AND id_insumo not in (SELECT codinsumo FROM ProyectoProductoInsumo WHERE codProducto = @producto)
			ORDER BY nomTipoInsumo

		end

	ELSE

		begin

			SELECT id_Insumo
			, nomTipoInsumo
			, nomInsumo
			, Presentacion
			, Unidad
			, id_TipoInsumo
			FROM ProyectoInsumo, TipoInsumo
			WHERE codTipoInsumo = id_TipoInsumo 
			AND codProyecto = @proyecto
			and id_tipoInsumo= @tipoinsumo
			AND id_insumo not in (SELECT codinsumo FROM ProyectoProductoInsumo WHERE codProducto = @producto)
			ORDER BY nomTipoInsumo

		end
END