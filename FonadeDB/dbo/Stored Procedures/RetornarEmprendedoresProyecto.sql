
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RetornarEmprendedoresProyecto]
	@Proyecto int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT id_contacto, nombres + ' ' +apellidos as nombre, email,Id_Contacto,CodProyecto from proyectocontacto p, contacto where id_contacto=codcontacto and p.inactivo=0 and codproyecto = @Proyecto  and codrol = 3 order by nombres
END