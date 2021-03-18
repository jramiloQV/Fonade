
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RetornarContactos]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select c.Nombres as Nombres,c.Telefono as Telefono,i.NomInstitucion as Institucion,c.Identificacion as Cedula from fonade.dbo.Contacto c, fonade.dbo.Institucion i where c.CodInstitucion=i.Id_Institucion and c.Id_Contacto<1000
END