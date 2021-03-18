
CREATE PROCEDURE MD_MostrarInfoProyectoImprimir
	@CodProyecto int
AS

BEGIN
	
	SELECT top 1 P.*
	, T.nomTipoProyecto
	, E.nomEstado
	, C.nomCiudad + '(' + D.nomDepartamento + ')' Lugar
	, N.Nombres + ' ' + N.Apellidos NomContacto
	, I.nomunidad +' (' +  I.nomInstitucion + ')' nomunidad
	, S.nomSubsector 
	, '$' + convert(varchar,cast((SM.SalarioMinimo * isnull(PF.Recursos,0)) as money),-1) as Recursos
	FROM Proyecto P 
	INNER JOIN TipoProyecto T  ON P.codTipoProyecto = T.Id_TipoProyecto 
	INNER JOIN Estado E ON E.id_estado = P.codEstado 
	INNER JOIN Ciudad C ON C.id_Ciudad = P.codCiudad 
	INNER JOIN Departamento D ON D.id_Departamento = C.codDepartamento 
	INNER JOIN Contacto N ON N.id_Contacto = P.codContacto
	INNER JOIN Institucion I ON I.id_Institucion = P.codInstitucion 
	INNER JOIN SubSector S ON S.id_SubSector = P.codSubSector 
	INNER JOIN ProyectoFinanzasIngresos PF ON PF.codproyecto = P.id_proyecto 
	inner join ConvocatoriaProyecto cp on cp.CodProyecto= p.Id_Proyecto
	inner join Convocatoria Conv on Conv.Id_Convocatoria= cp.CodConvocatoria
	inner join SalariosMinimos SM on SM.AñoSalario = year(Conv.FechaInicio)
	WHERE Id_Proyecto =@CodProyecto
	order by Conv.Id_Convocatoria desc

END