
CREATE PROCEDURE [dbo].[MD_VerEstudiosAsesor]
	@id_contacto int
AS

BEGIN
		SELECT CASE 
				WHEN CE.FlagIngresadoAsesor = 0 
					THEN 1
				WHEN CE.FlagIngresadoAsesor IS NULL
				THEN 1
				WHEN CE.FlagIngresadoAsesor = 1
				THEN 0
				ELSE  1
			END as FlagIngresadoAsesor,

		CE.Id_ContactoEstudio, CE.TituloObtenido,  ISNULL( CAST( CE.AnoTitulo as varchar(10) ) ,'En Curso') AnoTitulo, CE.Finalizado  , CE.Institucion, CE.CodCiudad, 
		C.NomCiudad, D.NomDepartamento, NE.NomNivelEstudio, codprogramaAcademico
		,'javascript:void(window.open("IngresarInformacionAcademica.aspx?LoadCode=' + cast(CE.Id_ContactoEstudio as varchar(15)) +  '","_blank","width=580,height=580,toolbar=no, scrollbars=no, resizable=no"));' as URL
		FROM ContactoEstudio CE, Ciudad C, Departamento D, NivelEstudio NE
		WHERE CE.CodCiudad = C.ID_Ciudad
		AND C.CodDepartamento = D.Id_Departamento
		AND CE.CodNivelEstudio = NE.Id_NivelEstudio
		AND codcontacto = @id_contacto
		ORDER BY finalizado, CE.AnoTitulo Desc
END