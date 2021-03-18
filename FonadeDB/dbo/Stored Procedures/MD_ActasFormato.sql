-- ================================================================================================
-- Author:		Jorge Martinez
-- Create date: 26/11/2014
-- Description:	Consulta los registros de acta y formato para una convocatoria
-- ================================================================================================
CREATE PROCEDURE [dbo].[MD_ActasFormato]
	@codConvocatoria int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT		ConvocatoriaActa.Id_Acta, ConvocatoriaActa.CodConvocatoria, ConvocatoriaActa.NomActa, ConvocatoriaActa.Fecha, ConvocatoriaActa.FechaActa, 
				DocumentoFormato.NomDocumentoFormato, ConvocatoriaActa.URL, DocumentoFormato.Icono, ConvocatoriaActa.CodDocumentoFormato, 
				ConvocatoriaActa.NumActa, ConvocatoriaActa.Comentario
	FROM	    ConvocatoriaActa INNER JOIN DocumentoFormato ON ConvocatoriaActa.CodDocumentoFormato = DocumentoFormato.Id_DocumentoFormato
	WHERE       (ConvocatoriaActa.Borrado = 0) AND (ConvocatoriaActa.CodConvocatoria = @codConvocatoria)
	ORDER BY ConvocatoriaActa.NomActa
END