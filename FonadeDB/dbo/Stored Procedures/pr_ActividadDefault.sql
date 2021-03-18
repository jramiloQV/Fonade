
CREATE PROCEDURE [dbo].[pr_ActividadDefault]
   @codproyecto int 
AS
BEGIN  
	-- Creacion: AndresGutierrez 11/10/2011
	-- declaramos las variables
	declare @actividad as int
	declare @nomact as varchar(150)
	declare @item as int
	declare @identifica as int

	-- declaramos un cursor llamado "CursorPO".
	declare CursorPO cursor for
		SELECT id_actdefault, Item, NomActividad FROM ActividadDefault
	open CursorPO  
	-- Avanzamos un registro y cargamos en las variables los valores encontrados en el primer registro
	fetch next from CursorPO into @actividad, @item, @nomact
		while @@fetch_status = 0
		begin
			INSERT INTO ProyectoActividadPOInterventor (NomActividad,CodProyecto,Item,Metas)
			VALUES (Replace(@nomact,'''',''), @codproyecto, @item, '')
			
			-- Se hace la consulta para obtener identificador
			SELECT @identifica = id_actividad FROM ProyectoActividadPOInterventor 
			WHERE CodProyecto = @CodProyecto AND NomActividad=Replace(@nomact,'''','')
			
			INSERT INTO ProyectoActividadPOMesInterventor (CodActividad,Mes,CodTipoFinanciacion,Valor)
			VALUES (@identifica, 1, 1, 0)

			-- Avanzamos otro registro
			fetch next from CursorPO into @actividad, @item, @nomact
		end
	-- cerramos el cursor
	close CursorPO
	deallocate CursorPO
END