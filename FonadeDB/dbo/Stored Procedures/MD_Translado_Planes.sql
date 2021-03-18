-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [MD_Translado_Planes]
	-- Add the parameters for the stored procedure here
	@CodProyecto int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		SELECT NomProyecto, NomUnidad, NomInstitucion, Id_Institucion, NomCiudad, Id_Ciudad, NomDepartamento, Id_Departamento, NomSubsector, Id_Subsector, NomSector, Id_Sector
		FROM 	 proyecto P, Institucion I, Ciudad C, Departamento D, Subsector SB, sector S
		WHERE  CodInstitucion = Id_Institucion
		AND 	 P.CodCiudad = Id_Ciudad
		AND 	 C.CodDepartamento = Id_Departamento
		AND 	 SB.Id_subsector = P.codSubsector
		AND 	 S.Id_Sector = SB.CodSector
		AND 	 Id_Proyecto = @CodProyecto
END