
create PROCEDURE MD_MostrarArchivosOffline
	@CodUsuario int
AS

BEGIN
	select id_documento, nomdocumento, fecha, url, nomdocumentoformato,'/images/IcoDocNormal.png' as  icono, CodDocumentoFormato, nomtab
	from  documentoformato, tab right outer join documento d
	on id_tab=d.codtab where id_documentoformato=coddocumentoformato and borrado=0
	and codproyecto =@CodUsuario
	and (CodDocumentoFormato = 19 or CodDocumentoFormato = 17) order by nomdocumento
END