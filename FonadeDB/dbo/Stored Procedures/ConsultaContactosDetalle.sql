/* =============================================
Author:		Wilson Guasca
alter date: 2010-06-23 12:50
Description: Consulta detallada de usuarios 
  incluyendo información del perfil (grupo)

  @Activos: Boolean que indica si se filtran todos los usuarios por el estado Inactivo
   0 : Inactivos
   1 : Activos
   NULL : Todos

EXECUTE [ConsultaContactosDetalle] 01
============================================= */
CREATE PROCEDURE [dbo].[ConsultaContactosDetalle]
	@Activos bit = NULL
AS
BEGIN

	SET NOCOUNT ON;
	SELECT C.Id_Contacto, C.Identificacion, 
		   C.Nombres + ' ' + C.Apellidos AS Nombres, 
		   ISNULL(CONVERT(varchar(20), LI.FechaUltimoIngreso, 120), '') As FechaUltimoIngreso, 
		   G.NomGrupo As Grupo, 
		   'Estado' = CASE ISNULL(C.Inactivo, 0)
			WHEN 1 THEN 'Inactivo'
			WHEN 0 THEN 'Activo'
		   END
	FROM   Contacto AS C INNER JOIN
		   GrupoContacto AS GC ON C.Id_Contacto = GC.CodContacto INNER JOIN
		   Grupo AS G ON GC.CodGrupo = G.Id_Grupo LEFT OUTER JOIN
		   LogIngreso AS LI ON C.Id_Contacto = LI.CodContacto
	WHERE  (@Activos IS NULL OR C.Inactivo = (1 - @Activos))
END