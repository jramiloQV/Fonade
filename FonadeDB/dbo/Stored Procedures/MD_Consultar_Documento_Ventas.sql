
CREATE PROCEDURE [dbo].[MD_Consultar_Documento_Ventas]
(
	@CodProduccion INT,
	@Mes INT
)
AS
select *
from AvanceVentasPOAnexos, documentoformato
where id_documentoformato=coddocumentoformato 
and borrado=0
and CodProducto = @CodProduccion and mes=@Mes
order by nomdocumento