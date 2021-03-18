
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RetornarPlanesNegocio]
@Email nvarchar(250)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select id_proyecto,nomproyecto, nomciudad, nomdepartamento from proyecto,ciudad,departamento where id_ciudad=codciudad and codcontacto = @Email and id_departamento=coddepartamento and inactivo=0 order by NomProyecto asc
END