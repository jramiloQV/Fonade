-- =============================================
-- Author:		Alberto Palencia B
-- Create date: 17/03/2014
-- Description:	Carga la informacion de la convocatorias para el listado de planes acreditados
-- =============================================
CREATE PROCEDURE MD_CargarConvocatorias_Estados
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	  Select Id_Convocatoria, NomConvocatoria from Convocatoria
		  where Id_Convocatoria in ((select distinct CodConvocatoria from ProyectoAcreditacionDocumento))
		  order by NomConvocatoria
 

 select Id_Estado,NomEstado from Estado where Id_Estado in (1,4,10,11,12,13)
 order by NomEstado


END