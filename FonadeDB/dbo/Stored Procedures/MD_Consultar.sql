
CREATE PROCEDURE  [dbo].[MD_Consultar]
	
	 @id_usuario int
	,@Cod_grupo int
	,@Cod_institucion int
	,@accion varchar(255)
	,@pclave  varchar(255)

AS
BEGIN

	
	IF @Cod_grupo = 4

		Begin

		SELECT  P.CodInstitucion, P.Inactivo, P.Id_Proyecto, P.NomProyecto, P.Sumario, S.NomSubSector, E.NomEstado, C.NomCiudad, D.NomDepartamento, P.FechaCreacion, I.NomUnidad + ' ('+I.nomInstitucion+')' Unidad 
				 ,  (Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto) as CodConvocatoria
				 FROM Proyecto P, SubSector S, Estado E, Ciudad C, Departamento D, Institucion I 
			 
				 WHERE 
				 (P.NomProyecto like  '%' + @pclave + '%' 
				  OR P.Sumario like  '%' + @pclave + '%' 
				  OR S.NomSubSector like '%' + @pclave + '%' 
				  OR D.NomDepartamento like  '%' + @pclave + '%'  
				  OR C.NomCiudad like  '%' + @pclave + '%' ) 
				 AND S.Id_SubSector = P.CodSubSector 
				 AND I.Id_Institucion = P.CodInstitucion 
				 AND E.Id_estado = P.CodEstado 
				 AND C.Id_Ciudad = P.CodCiudad 
				 AND C.CodDepartamento = D.Id_Departamento 
				 and p.inactivo=0
				 and P.CodInstitucion=@Cod_institucion

		end
	ELSE

				SELECT  P.CodInstitucion, P.Inactivo, P.Id_Proyecto, P.NomProyecto, P.Sumario, S.NomSubSector, E.NomEstado, C.NomCiudad, D.NomDepartamento, P.FechaCreacion, I.NomUnidad + ' ('+I.nomInstitucion+')' Unidad 
				 ,  (Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto) as CodConvocatoria
				 FROM Proyecto P, SubSector S, Estado E, Ciudad C, Departamento D, Institucion I 
			 
				 WHERE 
				 (P.NomProyecto like  '%' + @pclave + '%' 
				  OR P.Sumario like  '%' + @pclave + '%' 
				  OR S.NomSubSector like '%' + @pclave + '%' 
				  OR D.NomDepartamento like  '%' + @pclave + '%'  
				  OR C.NomCiudad like  '%' + @pclave + '%' ) 
				 AND S.Id_SubSector = P.CodSubSector 
				 AND I.Id_Institucion = P.CodInstitucion 
				 AND E.Id_estado = P.CodEstado 
				 AND C.Id_Ciudad = P.CodCiudad 
				 AND C.CodDepartamento = D.Id_Departamento 
				 and p.inactivo=0
END