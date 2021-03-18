-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [MD_InfoContactoEmprendedor]
	-- Add the parameters for the stored procedure here
	@CodContacto int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		SELECT C.nombres,C.apellidos,TI.NOMTIPOIDENTIFICACION,C.identificacion, C.email, C.telefono, TA.NOMTIPOAPRENDIZ,C.Genero,C.FechaNacimiento,C1.NomCiudad 'NomCiudadNacimiento',D1.NomDepartamento 'NomDepartamentoNacimiento',C.Telefono,C2.NomCiudad 'NomCiudadExpedicion', D2.NomDepartamento 'NomDepartamentoExpedicion'
		FROM Contacto C
		JOIN TIPOIDENTIFICACION TI ON (C.CODTIPOIDENTIFICACION = TI.ID_TIPOIDENTIFICACION)
		LEFT JOIN CIUDAD C1 ON (C1.ID_CIUDAD = C.CODCIUDAD)
		LEFT JOIN CIUDAD C2 ON (C2.ID_CIUDAD= C.LUGAREXPEDICIONDI)
		LEFT JOIN DEPARTAMENTO D1 ON (D1.ID_DEPARTAMENTO = C1.CODDEPARTAMENTO)
		LEFT JOIN DEPARTAMENTO D2 ON (D2.ID_DEPARTAMENTO = C2.CODDEPARTAMENTO)
		LEFT JOIN TIPOAPRENDIZ TA ON (TA.ID_TIPOAPRENDIZ = C.CODTIPOAPRENDIZ)
		WHERE C.ID_Contacto=@CodContacto
END