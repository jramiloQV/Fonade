/* 
	Autor  alberto Palencia
	Creado 25-02-2014
	Obtengo los integrantes de la iniciativa

	exec MD_GetIntegrantesIniciativa  50529,141

*/
CREATE procedure [dbo].[MD_GetIntegrantesIniciativa](
	@codProyecto varchar(50) = 0,
	@codConvocatoria varchar(50)	
)

as
begin

declare @rolemprendedor int = 3 -- esta es una variable constantes

SELECT top 1

	C.id_contacto
	,C.Nombres + ' ' + C.apellidos NomCompleto
	,PC.Beneficiario
	,ISNULL(EC.AporteDinero,0) as AporteDinero
	,ISNULL(EC.AporteEspecie,0) as AporteEspecie
	,EC.DetalleEspecie
FROM Contacto C INNER JOIN ProyectoContacto PC
ON C.id_contacto = PC.codContacto 
and Pc.inactivo=0 and C.Inactivo = 0 
and PC.codProyecto = @codProyecto and codRol = @rolemprendedor
LEFT JOIN EvaluacionContacto EC ON PC.codContacto = EC.codContacto
and PC.codProyecto = EC.codProyecto  and EC.codConvocatoria= @codConvocatoria
ORDER BY C.Nombres + ' ' + C.apellidos
end