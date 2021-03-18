
CREATE PROCEDURE [dbo].[MD_cargarProyectosInterv]

	@CONST_RolInterventor int,
	@CodInterventor int,
	@CONST_RolInterventorLider int


AS

BEGIN



	
		SELECT distinct (p.id_proyecto), p.NomProyecto, p.Sumario FROM Proyecto p
inner join proyectocontacto pc on p.id_proyecto=pc.CodProyecto and 
pc.inactivo=0 --and Codrol=6 and pc.CodContacto='77963'
inner join empresa e on e.codproyecto = p.Id_Proyecto
inner join EmpresaInterventor ei on ei.CodEmpresa= e.id_empresa
INNER JOIN Rol r ON ei.Rol = r.Id_Rol
where ei.Inactivo = 0
and ei.Rol in (@CONST_RolInterventor,@CONST_RolInterventorLider)
and ei.CodContacto = @CodInterventor
END