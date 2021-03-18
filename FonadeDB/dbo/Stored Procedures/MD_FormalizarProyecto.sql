

CREATE PROCEDURE [dbo].[MD_FormalizarProyecto]
	@id_usuario int,
	@CONST_Anexos int,
	@CodInstitucion int,
	@CONST_Inscripcion int
AS

BEGIN

	Declare @ConteoCod int
	set @ConteoCod = (SELECT isnull(COUNT(0),0) as Total FROM Tab WHERE codTab is NULL and IdVersionProyecto = 1 and id_tab < 20) - 1
	Declare @ConteoCodV2 int
	set @ConteoCodV2 = (SELECT isnull(COUNT(0),0) as Total FROM Tab WHERE codTab is NULL and IdVersionProyecto = 2 and id_tab < 76) - 1

	SELECT id_proyecto, nomproyecto, nomciudad +' ('+nomdepartamento+')' Lugar, codEstado, ' ' as nomconvocatoria, 0 as id_proyectoformalizacion, 'Formalizar' as Adicional, 'ProyectoFormalizar2.aspx?CodProyecto=' as URL
	FROM Proyecto R, TabProyecto P, Tab T, Ciudad C, Departamento D 
	WHERE R.id_proyecto=P.codProyecto AND T.id_tab=P.codTab AND id_tab<>19 and T.codTab is NULL AND R.codCiudad=C.id_ciudad 
	AND C.codDepartamento=D.id_departamento AND R.codinstitucion=@CodInstitucion and R.inactivo=0 and R.IdVersionProyecto = 1
	GROUP BY id_proyecto, nomproyecto, nomciudad, nomdepartamento, codEstado
	HAVING sum(convert(int,realizado))=@ConteoCod  and codestado =@CONST_Inscripcion
	UNION
	SELECT id_proyecto, nomproyecto, nomciudad +' ('+nomdepartamento+')' Lugar, codEstado, ' ' as nomconvocatoria, 0 as id_proyectoformalizacion, 'Formalizar' as Adicional, 'ProyectoFormalizar2.aspx?CodProyecto=' as URL
	FROM Proyecto R, TabProyecto P, Tab T, Ciudad C, Departamento D 
	WHERE R.id_proyecto=P.codProyecto AND T.id_tab=P.codTab AND id_tab<>75 and T.codTab is NULL AND R.codCiudad=C.id_ciudad 
	AND C.codDepartamento=D.id_departamento AND R.codinstitucion=@CodInstitucion and R.inactivo=0 and R.IdVersionProyecto = 2 
	GROUP BY id_proyecto, nomproyecto, nomciudad, nomdepartamento, codEstado
	HAVING sum(convert(int,realizado))=@ConteoCodV2  and codestado =@CONST_Inscripcion	
	UNION
	SELECT id_proyecto, nomproyecto, nomciudad +' ('+nomdepartamento+')' Lugar, codEstado, nomconvocatoria,id_proyectoformalizacion, 'Imprimir' as Adicional, 'verFormalizacion.aspx?CodProyecto=' as URL
	FROM Proyecto P,  Ciudad C, Departamento D, convocatoria RIGHT OUTER JOIN ProyectoFormalizacion F
	ON id_convocatoria=f.codconvocatoria
	WHERE  P.id_proyecto=F.codProyecto AND  P.Codinstitucion=@CodInstitucion and P.codCiudad=C.id_ciudad 
	AND C.codDepartamento=D.id_departamento AND F.codContacto=@id_usuario

	order by Adicional, nomproyecto

END