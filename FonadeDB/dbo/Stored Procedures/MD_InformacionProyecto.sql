-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_InformacionProyecto]
	-- Add the parameters for the stored procedure here
	@CodProyecto int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT P.*, T.nomTipoProyecto, Pf.Recursos, E.nomEstado, C.nomCiudad + '(' + D.nomDepartamento + ')' Lugar, N.Nombres + ' ' + N.Apellidos NomContacto, I.nomunidad, I.nomInstitucion, S.nomSubsector
	FROM Proyecto P
	INNER JOIN TipoProyecto T  ON P.codTipoProyecto = T.Id_TipoProyecto
	INNER JOIN Estado E ON E.id_estado = P.codEstado
	INNER JOIN Ciudad C ON C.id_Ciudad = P.codCiudad
	INNER JOIN Departamento D ON D.id_Departamento = C.codDepartamento
	INNER JOIN Contacto N ON N.id_Contacto = P.codContacto
	INNER JOIN Institucion I ON I.id_Institucion = P.codInstitucion
	INNER JOIN SubSector S ON S.id_SubSector = P.codSubSector
	INNER JOIN ProyectoFinanzasIngresos PF ON PF.codproyecto = P.id_proyecto
	WHERE Id_Proyecto = @CodProyecto
END