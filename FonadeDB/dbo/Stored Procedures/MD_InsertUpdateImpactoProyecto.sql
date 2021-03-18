
CREATE PROCEDURE MD_InsertUpdateImpactoProyecto
	@codigoProyecto int,
	@textoImpacto text
AS

BEGIN
	declare @conteo int
	select @conteo=count(*) from ProyectoImpacto where codproyecto=@codigoProyecto

	IF @conteo=0

		begin
	
			insert into ProyectoImpacto (CodProyecto,Impacto)
			values (@codigoProyecto,@textoImpacto)

		end

	ELSE

		begin
	
			Update ProyectoImpacto 
			set Impacto = @textoImpacto
			where codproyecto=@codigoProyecto

		end
END