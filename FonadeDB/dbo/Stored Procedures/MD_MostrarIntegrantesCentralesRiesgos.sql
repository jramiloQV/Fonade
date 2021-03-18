
CREATE PROCEDURE [dbo].[MD_MostrarIntegrantesCentralesRiesgos]
	@CodProyecto int,
	@rolEmprendedor int,
	@CodConvocatoria int
AS

BEGIN

	SELECT 
		distinct C.id_contacto
		, C.Nombres + ' ' + C.apellidos NomCompleto
		, EC.Entidades
		, EC.PeorCalificacion
		, EC.CuentasCorrientes
		, EC.ValorCartera
		, EC.ValorOtrasCarteras 
	FROM Contacto C 
	INNER JOIN ProyectoContacto PC 
		ON C.id_contacto = PC.codContacto 
		and pc.inactivo=0 and C.Inactivo = 0 
		and PC.codProyecto = @CodProyecto 
		and codRol = @rolEmprendedor
	LEFT JOIN EvaluacionContacto EC 
		ON PC.codContacto = EC.codContacto 
		and PC.codProyecto = EC.codProyecto  
		and EC.codConvocatoria=@CodConvocatoria
	ORDER BY C.Nombres + ' ' + C.apellidos

END