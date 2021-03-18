-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_ProyectosEjecucion]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
			SELECT Proyecto.Id_Proyecto, Proyecto.NomProyecto, Empresa.Id_Empresa, Empresa.RazonSocial AS NomEmpresa, Interventor.CodContacto, Interventor.CodCoordinador,
			Contacto_1.Nombres + ' ' + Contacto_1.Apellidos AS NomCoordinador,
			Contacto.Nombres + ' ' + Contacto.Apellidos AS NomInterventor,
			Contacto.Email AS EmailInterventor, Contacto_1.Email AS EmailCoordinador
			FROM Proyecto
			INNER JOIN Empresa ON Proyecto.Id_Proyecto = Empresa.codproyecto
			INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa
			INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto
			INNER JOIN Contacto ON Interventor.CodContacto = Contacto.Id_Contacto
			INNER JOIN Contacto Contacto_1 ON Interventor.CodCoordinador = Contacto_1.Id_Contacto
			WHERE (Proyecto.CodEstado = 7)
			AND (EmpresaInterventor.Rol = 8)
			AND (EmpresaInterventor.Inactivo = 0)
			ORDER BY Proyecto.Id_Proyecto --Añadido el 04/06/2014.
END