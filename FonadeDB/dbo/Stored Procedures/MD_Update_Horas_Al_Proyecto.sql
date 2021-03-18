
create PROCEDURE MD_Update_Horas_Al_Proyecto
	@HorasProyecto int,
	@CodProyecto int,
	@CodContacto int,
	@Codrol tinyint
AS

BEGIN

	update proyectocontacto 
	set horasproyecto=@HorasProyecto 
	where codproyecto=@CodProyecto 
	and codcontacto=@CodContacto 
	and codrol=@Codrol

END