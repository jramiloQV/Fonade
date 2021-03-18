-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_InterventorFida]
	-- Add the parameters for the stored procedure here
	@CONST_RolInterventorLider int,
	@CodUsuario int,
	@Id_Empresa int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT Contacto.Nombres + ' ' + Contacto.Apellidos AS Intervemtor
	FROM Contacto
	INNER JOIN EmpresaInterventor ON Contacto.Id_Contacto = EmpresaInterventor.CodContacto
	INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto
	WHERE (EmpresaInterventor.Rol = 8)
	AND (EmpresaInterventor.Inactivo = 0)
	AND (Interventor.CodCoordinador = 47876)
	--AND (EmpresaInterventor.CodEmpresa = @Id_Empresa)
END