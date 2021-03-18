-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [MD_Consultarjefe]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		SELECT T.NomTipoIdentificacion, C.Identificacion, C.Nombres, C.Apellidos, C.Email, I.NomInstitucion, I.NomUnidad, NomCiudad, NomDepartamento
		FROM TipoIdentificacion T, Contacto C, Institucion I, InstitucionContacto IC, Ciudad, Departamento
		WHERE T.Id_TipoIdentificacion = C.CodTipoIdentificacion
		AND CodDepartamento = Id_Departamento
		AND I.CodCiudad = Id_Ciudad 
		AND IC.CodContacto = C.Id_Contacto
		AND I.Id_Institucion = IC.CodInstitucion
		AND IC.FechaFin IS NULL
END