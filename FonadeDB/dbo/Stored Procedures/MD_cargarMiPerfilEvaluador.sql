
CREATE PROCEDURE [dbo].[MD_cargarMiPerfilEvaluador]
	@id_contacto int
AS

BEGIN
	SELECT CASE WHEN e.Persona = 'J' THEN 'Jurídica'
				WHEN e.Persona = 'N' THEN 'Natural'
					END as Persona, c.Nombres, c.Apellidos, ti.nomtipoidentificacion
	, c.identificacion, c.email, c.Direccion, c.Telefono, c.fax, c.Experiencia, c.Intereses, c.HojaVida
	,e.codBanco, e.CodTipoCuenta, e.Cuenta, e.MaximoPlanes, e.ExperienciaPrincipal, e.ExperienciaSecundaria
	FROM Contacto c
	inner join Tipoidentificacion ti on ti.Id_TipoIdentificacion= c.CodTipoIdentificacion
	inner join Evaluador e on e.CodContacto= c.Id_Contacto
	WHERE c.Id_Contacto = @id_contacto
END