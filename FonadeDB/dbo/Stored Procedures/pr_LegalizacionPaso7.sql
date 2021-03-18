CREATE PROCEDURE [dbo].[pr_LegalizacionPaso7]
   @codproyecto int, 
   @inscripcion int,
   @rol int
AS
BEGIN  

 -- declaramos las variables
  declare @contacto as int
    
  UPDATE Proyecto SET CodEstado = @inscripcion WHERE Id_proyecto = @codproyecto

  declare CursorPO cursor for
  SELECT CodContacto FROM ProyectoContacto WHERE CodProyecto = @codproyecto AND Inactivo = 0 AND CodRol = @rol
  open CursorPO
  
  -- Avanzamos un registro y cargamos en las variables los valores encontrados en el primer registro
  fetch next from CursorPO
  into @contacto
      while @@fetch_status = 0
		begin
		UPDATE Contacto SET InactivoAsignacion = 1 WHERE id_Contacto = @contacto
		 

		-- Avanzamos otro registro
		fetch next from CursorPO
		into @contacto
		end
    
	-- cerramos el cursor
	close CursorPO
	deallocate CursorPO

	

END