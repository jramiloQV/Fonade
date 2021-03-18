-- =============================================
-- Author:		Alberto Palencia Benedetti
-- Create date: 16/03/2014
-- Description:	Obtiene la informacion del emprendedor por el id contacto
-- MD_ObtenerInformacionEmprendedor 77241
-- =============================================
CREATE PROCEDURE [dbo].[MD_ObtenerInformacionEmprendedor]
	@CodContacto INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
SELECT
	C.ID_Contacto as codigoContactoW,
	C.nombres,
	C.apellidos,
	TI.NOMTIPOIDENTIFICACION,
	C.identificacion,
	C.email,
	C.telefono,
	TA.NOMTIPOAPRENDIZ,
	Case when C.Genero = 'F' then 'Femenino' else 'Masculino' end Genero,
	C.FechaNacimiento,
	C1.NomCiudad,
	D1.NomDepartamento,
	C.Telefono,
	(C2.NomCiudad  + ' - ' + D2.NomDepartamento ) 'Expedicion'
FROM Contacto C
JOIN TIPOIDENTIFICACION TI ON (C.CODTIPOIDENTIFICACION = TI.ID_TIPOIDENTIFICACION)
LEFT JOIN CIUDAD C1 ON (C1.ID_CIUDAD = C.CODCIUDAD)
LEFT JOIN CIUDAD C2 ON (C2.ID_CIUDAD= C.LUGAREXPEDICIONDI)
LEFT JOIN DEPARTAMENTO D1 ON (D1.ID_DEPARTAMENTO = C1.CODDEPARTAMENTO)
LEFT JOIN DEPARTAMENTO D2 ON (D2.ID_DEPARTAMENTO = C2.CODDEPARTAMENTO)
LEFT JOIN TIPOAPRENDIZ TA ON (TA.ID_TIPOAPRENDIZ = C.CODTIPOAPRENDIZ)
WHERE C.ID_Contacto=@CodContacto

SELECT  
		CE.TITULOOBTENIDO
		,CE.INSTITUCION
		,CE.CODCIUDAD
		,NE.NOMNIVELESTUDIO
		,CE.CODPROGRAMAACADEMICO
	     , CASE WHEN CE.FINALIZADO = 1 
			THEN 'Finalizado' ELSE 'Actualmente en curso'  END Finalizado
	  ,CE.FECHAINICIO
	  ,CE.FECHAFINMATERIAS
	  ,CE.FECHAGRADO
	  ,CE.FECHAULTIMOCORTE
	  ,CE.SEMESTRESCURSADOS
	  ,C.NOMCIUDAD
	  ,PA.NOMPROGRAMAACADEMICO

FROM CONTACTOESTUDIO CE JOIN CIUDAD C ON (C.ID_CIUDAD = CE.CODCIUDAD)
									JOIN PROGRAMAACADEMICO PA ON (PA.ID_PROGRAMAACADEMICO = CE.CODPROGRAMAACADEMICO)
									JOIN NIVELESTUDIO NE ON (NE.ID_NIVELESTUDIO = CE.CODNIVELESTUDIO)
									WHERE CODCONTACTO = @CodContacto
ORDER BY FLAGINGRESADOASESOR DESC, FINALIZADO DESC, FECHAINICIO ASC
						
END