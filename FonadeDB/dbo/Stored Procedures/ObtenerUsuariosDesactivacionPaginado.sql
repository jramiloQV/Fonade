
/* =============================================
Author:		Wilson Guasca
alter date: 2010-06-21 14:40
Description: Selección 'paginada' de registros de la tabla
  [ServicioUsuariosDesactivados]
  @CodDesactivacion : Id del proceso para filtrar
  @CodRegistro : Id de la tabla a partir del cual 
    se hará el conteo para 'paginar'
  @PerPage : Número máximo de registros a retornar

EXECUTE [DesactivarUsuariosPorFecha] '2010-06-05 00:05', '2010-06-21 11:50:38'
============================================= */
CREATE PROCEDURE [dbo].[ObtenerUsuariosDesactivacionPaginado]
	@CodDesactivacion int,
	@CodRegistro int = 0,
	@PerPage int = 20
AS
BEGIN

	SET NOCOUNT ON;
	SELECT TOP (@PerPage) 
		   SUD.Id_ServicioUsuariosDesactivados, SUD.CodContacto, 
		   ISNULL(SUD.FechaUltimoAcceso, 0) As FechaUltimoAcceso, SUD.FechaNotificacion, C.Nombres, 
		   C.Apellidos, C.Email, C.fechaCreacion
	FROM   ServicioUsuariosDesactivados AS SUD INNER JOIN
		   Contacto AS C ON SUD.CodContacto = C.Id_Contacto
	WHERE  (SUD.CodServicioRegistroDesactivacion = @CodDesactivacion)
			 AND (SUD.Id_ServicioUsuariosDesactivados > @CodRegistro)
			 AND (SUD.EnvioExitoso = 0)
END