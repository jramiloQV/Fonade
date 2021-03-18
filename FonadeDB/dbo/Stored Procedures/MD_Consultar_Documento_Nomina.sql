
create PROCEDURE  [dbo].[MD_Consultar_Documento_Nomina]
	

	@CodCargo int
	,@Mes int


AS
BEGIN

	
	

		Begin

		select * from AvanceCargoPOAnexos, documentoformato 
	where id_documentoformato=coddocumentoformato and borrado=0 
	and CodCargo =@CodCargo and mes=@Mes  order by nomdocumento
		end
	
END