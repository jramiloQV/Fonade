
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RetornarEstudiosAsesor]
@Email nvarchar(250)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CE.Id_ContactoEstudio, CE.TituloObtenido, CE.AnoTitulo, CE.Finalizado, CE.Institucion, CE.CodCiudad, C.NomCiudad, D.NomDepartamento, NE.NomNivelEstudio, Case NE.Id_NivelEstudio When 3 Then 2 When 7 Then 2 When 8 Then 2 ELSE 1 END Grupo FROM ContactoEstudio CE, Ciudad C, Departamento D, NivelEstudio NE,Contacto ct WHERE CE.CodCiudad = C.ID_Ciudad AND C.CodDepartamento = D.Id_Departamento AND CE.CodNivelEstudio = NE.Id_NivelEstudio AND ct.Id_Contacto=CodContacto and ct.Id_Contacto=@Email
END