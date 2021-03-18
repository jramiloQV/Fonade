
CREATE PROCEDURE MD_MostrarProyectosPorConvocatoria
	@IdConvocatoria int
AS

BEGIN
	
	SELECT isnull(F.cofinanciacion, 0) Cofinanciacion
	, C.nomCiudad + ' (' + D.nomDepartamento + ')' ciudad
	, S.nomSector
	, P.nomProyecto
	, id_Proyecto
	, isnull(L.nombres + ' '+ L.Apellidos, '(No asignado)') Lider
	, I.NomUnidad + '('+I.nomInstitucion+')' Unidad
	, '$' + convert(varchar,cast((SM.SalarioMinimo * isnull(PF.Recursos,0)) as money),-1) as Recursos
	FROM ConvocatoriaProyecto CP 
	INNER JOIN Proyecto P ON  CP.codProyecto = P.id_Proyecto and CP.codConvocatoria=@IdConvocatoria
	inner join Convocatoria Conv on Conv.Id_Convocatoria= cp.CodConvocatoria
	INNER JOIN Institucion I ON  I.id_institucion = P.codinstitucion 
	INNER JOIN institucioncontacto IC ON  I.id_institucion = IC.codInstitucion and IC.fechafin is null 
	LEFT  JOIN ProyectoContacto CL ON P.id_Proyecto = CL.codProyecto AND CL.inactivo=0 and CL.codRol=1 
	LEFT  JOIN Contacto L ON CL.codContacto = L.id_Contacto 
	INNER JOIN Subsector Z ON P.codSubsector=Z.id_SubSector 
	INNER JOIN Sector S ON Z.codSector=S.id_Sector 
	INNER JOIN Ciudad C ON P.codCiudad=C.id_Ciudad 
	INNER JOIN Departamento D ON C.codDepartamento=D.id_Departamento 
	LEFT JOIN ProyectoFinanzasIngresos PF ON CP.codProyecto = PF.codProyecto 
	LEFT JOIN ConvocatoriaCofinanciacion F ON CP.codConvocatoria=F.codConvocatoria and C.id_Ciudad=F.codCiudad 
	inner join SalariosMinimos SM on SM.AñoSalario = year(Conv.FechaInicio)
	ORDER BY isnull(F.cofinanciacion, 0) desc, C.nomCiudad, D.nomDepartamento, S.nomSector, P.nomProyecto

END