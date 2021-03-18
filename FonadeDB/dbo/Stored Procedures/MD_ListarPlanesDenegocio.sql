-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [MD_ListarPlanesDenegocio]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 	Id_Proyecto, NomProyecto, NomUnidad, Id_ciudad, CodSubsector, Id_sector
	FROM 	proyecto, Institucion, Ciudad, Subsector, sector
	WHERE proyecto.codciudad = id_ciudad
	AND CodInstitucion = Id_Institucion
	AND Id_subsector = codSubsector
	AND Id_Sector = CodSector
END