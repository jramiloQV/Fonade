-- =============================================
-- Author:		Alberto Palencia Benedetti
-- Create date: 24/03/2014
-- Description:	Obtengo el listado de las empresas que tiene el interventor
-- =============================================
CREATE PROCEDURE [dbo].[MD_ListarEmpresaInterventor]
	-- Add the parameters for the stored procedure here
	@codusuario int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT distinct p.Id_Proyecto, p.NomProyecto, e.Id_Empresa, e.razonsocial
	FROM Empresa e, Proyecto p, EmpresaInterventor ei
	WHERE e.codproyecto = p.Id_Proyecto AND e.id_empresa = ei.CodEmpresa
	AND ei.CodContacto = @codusuario AND ei.Inactivo = 0
	ORDER BY NomProyecto
		

END