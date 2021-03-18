-- =============================================
-- Author:		alberto Palencia Benedetti
-- Create date: 15 / 03/ 2014
-- Description:	actualiza y quita el proyecto a los coordinadores y evaluadores.
-- =============================================
CREATE PROCEDURE MD_ActualizarProyectoNegocio
	@idproyecto int 
AS
BEGIN
	
	SET NOCOUNT ON;

  -- Pasar el proyecto a estado de registro y asesoria manteniendo los emprendedores y asesores
    update proyecto set codestado= 1 where id_proyecto= @idproyecto
  
   --  'Quitar aprobación del asesor
     update tabproyecto set realizado=0 where codproyecto= @idproyecto

    -- Quitar evaluador y coordinador del proyecto
	-- rol evaluador 4 rol coordinador 5

    update proyectocontacto set fechafin=getdate()
	,inactivo=1 Where (codrol= 4 or codrol= 5 ) and inactivo=0 and codproyecto= @idproyecto


END