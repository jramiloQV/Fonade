
	--txtSQL = 	" SELECT NomProyecto, NomUnidad, NomInstitucion, Id_Institucion, NomCiudad, Id_Ciudad, NomDepartamento, Id_Departamento, NomSubsector, Id_Subsector, NomSector, Id_Sector"&_
	--		" FROM 	 proyecto P, Institucion I, Ciudad C, Departamento D, Subsector SB, sector S "&_
	--		" WHERE  CodInstitucion = Id_Institucion "&_
	--		" AND 	 P.CodCiudad = Id_Ciudad "&_
	--		" AND 	 C.CodDepartamento = Id_Departamento "&_
	--		" AND 	 SB.Id_subsector = P.codSubsector "&_
	--		" AND 	 S.Id_Sector = SB.CodSector "&_
	--		" AND 	 Id_Proyecto = "&CodProyecto
	--'------------------------------------------------------
	
	CREATE procedure spGestionTranslados
	
	@Opcion Varchar(5),
	@Id_Proyecto int = null,
	@NombrePlan Varchar(300) = null,
	
	@NombreUnidad int = null,
	@Sector int = null,
	@Subsector int = null,
	@Departamento int = null,
	@Ciudad int 
	  
	
	as
	
	DECLARE @Tsql nvarchar(3000)
	DECLARE @Select nvarchar(1000)
	DECLARE @From nvarchar(1000)
	DECLARE @Where nvarchar(1000)

	DECLARE @Params nvarchar(100)
	
	
IF @Opcion = 'F' -- FILTRO BUSQUEDA
BEGIN

SET @Params = ' @Id_Proyecto int = null, @NombrePlan Varchar(300) = null'

	set @Select = ' SELECT NomProyecto, NomUnidad, NomInstitucion, Id_Institucion, NomCiudad, Id_Ciudad, NomDepartamento, Id_Departamento, NomSubsector, Id_Subsector, NomSector, Id_Sector '
	set @From =	' FROM 	 proyecto P, Institucion I, Ciudad C, Departamento D, Subsector SB, sector S '
	set @Where = '  WHERE  CodInstitucion = Id_Institucion
					AND 	 P.CodCiudad = Id_Ciudad
					AND 	 C.CodDepartamento = Id_Departamento			
					AND 	 SB.Id_subsector = P.codSubsector
					AND 	 S.Id_Sector = SB.CodSector '			
					

		if @Id_Proyecto is not null			 
			begin
				set @Where = @Where + ' AND Id_Proyecto = @Id_Proyecto '
			end 	
				 
		if @NombrePlan is not null			 
			begin
				set @Where = @Where + ' AND NomProyecto = @NombrePlan '
			end	
			
		SET @Tsql = @Select + @From + @Where
		
		--select @Tsql
			 
	 Execute sp_executesql @Tsql, @Params, @Id_Proyecto = @Id_Proyecto,@NombrePlan = @NombrePlan

END		

IF @Opcion = 'F1' -- FILTRO BUSQUEDA
BEGIN

	SELECT Id_Proyecto, NomProyecto, NomUnidad, NomInstitucion, Id_Institucion, NomCiudad, Id_Ciudad, NomDepartamento, Id_Departamento, NomSubsector, Id_Subsector, NomSector, Id_Sector 
	FROM 	 proyecto P, Institucion I, Ciudad C, Departamento D, Subsector SB, sector S 
	WHERE  CodInstitucion = Id_Institucion
					AND 	 P.CodCiudad = Id_Ciudad
					AND 	 C.CodDepartamento = Id_Departamento			
					AND 	 SB.Id_subsector = P.codSubsector
					AND 	 S.Id_Sector = SB.CodSector 					
					AND Id_Proyecto = @Id_Proyecto 



END


IF @Opcion = 'PROY'
BEGIN

SELECT ID_PROYECTO, NomProyecto FROM PROYECTO

END	

IF @Opcion = 'DEP'
BEGIN

SELECT Id_Departamento, NomDepartamento FROM Departamento

END			


IF @Opcion = 'CIU'
BEGIN

SELECT NomCiudad, Id_Ciudad FROM Ciudad

END			


IF @Opcion = 'SEC'
BEGIN

SELECT NomSector, Id_Sector  FROM sector

END			


IF @Opcion = 'SSEC'
BEGIN

SELECT  NomSubsector, Id_Subsector FROM Subsector

END	


IF @Opcion = 'UPD'
BEGIN

UPDATE proyecto SET 
			 CodCiudad = @Ciudad,
			 CodSubsector = @Subsector
			 WHERE Id_Proyecto= @Id_Proyecto
END