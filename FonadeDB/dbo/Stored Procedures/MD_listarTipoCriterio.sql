CREATE PROCEDURE [dbo].[MD_listarTipoCriterio]
	

AS

BEGIN


  select id_InterventorInformeFinalCriterio, nomInterventorInformeFinalCriterio from InterventorInformeFinalCriterio WHERE CodEmpresa=0


	
END