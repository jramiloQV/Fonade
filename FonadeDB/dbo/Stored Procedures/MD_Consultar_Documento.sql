
CREATE PROCEDURE  [dbo].[MD_Consultar_Documento]
	

	@CodActividad int
	,@Mes int


AS
BEGIN

	
	

		Begin

		select * 
from AvanceActividadPOAnexos, documentoformato 
where id_documentoformato=coddocumentoformato and borrado=0 
and CodActividad = @CodActividad and mes=@Mes order by nomdocumento

		end
	
END