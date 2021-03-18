create PROCEDURE MD_AsignacionActaProyecto

	@LegalizacionContrato int,
	@NumAct int,
	@CodProyecto int,
	@asignar int,
	@caso varchar(10)

AS

BEGIN

	INSERT INTO AsignacionActaProyecto (CodActa, CodProyecto, Asignado)
	VALUES(@NumAct, @CodProyecto,@asignar)

	IF @caso='update'
	
	begin
		update proyecto set codestado=@LegalizacionContrato where id_proyecto=@CodProyecto
	end

	

END