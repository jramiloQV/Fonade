-- =============================================
-- Author:		Alberto Palencia Benedetti
-- Create date:  2014-03-14
-- Description:	Obtiene todas las actas
-- =============================================
CREATE PROCEDURE MD_ObtenerActas
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
			SELECT 
				 E.Id_Acta
				,E.NumActa
				,E.NomActa
				,c.NomConvocatoria
				,e.publicado
			FROM evaluacionacta e JOIN convocatoria c ON  id_convocatoria=codconvocatoria
			ORDER by  NumActa

END