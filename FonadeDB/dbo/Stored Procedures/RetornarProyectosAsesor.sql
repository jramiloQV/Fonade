
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RetornarProyectosAsesor]
	@Email varchar(250)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT id_proyecto,nomproyecto, codinstitucion, codestado, nomunidad, nominstitucion, nomciudad, nomdepartamento, p.inactivo from proyecto p,ciudad,departamento,institucion where id_ciudad=p.codciudad and id_departamento=coddepartamento and id_institucion=codinstitucion and  exists (select codproyecto from proyectocontacto pc  where id_proyecto=codproyecto and pc.CodContacto=@Email)
END